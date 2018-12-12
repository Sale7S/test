using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COCAS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COCAS.Controllers
{
    public class FilesController : BaseController
    {
        private readonly COCASContext _context;

        public FilesController(COCASContext context)
        {
            _context = context;
        }

        public IActionResult Upload()
        {
            if (!IsStaff())
                return RedirectToAction("Login_Ar","Users");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login_Ar", "Users");

            if (ModelState.IsValid)
            {
                var csv_files = IsCSV(files);
                await ReadCSVFiles(csv_files);
                
                return RedirectToAction(nameof(UploadedFiles), csv_files);
            }

            return View();
        }
        
        private List<IFormFile> IsCSV(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var csv_files = new List<IFormFile>();

            foreach (var f in files)
            {
                if (f.FileName.EndsWith(".csv"))
                    if (f.Length > 0)
                        csv_files.Add(f);
            }

            return csv_files;
        }
        
        private async Task ReadCSVFiles(List<IFormFile> csv_files)
        {
            foreach (var f in csv_files)
            {
                var lines = new List<string>(); //Lines per file.
                var student_lines = new List<string>(); //Lines with students info.
                var student_dept_lines = new List<string>(); //Lines with students' departments info.
                var students_sections = new List<int>(); //Number of sections per students.
                var section = new List<Section>();

                using (var reader = new StreamReader(f.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        lines.Add(reader.ReadLine());
                        var curr = lines.Count - 1;

                        //'الطالب' is a unique word in the student's info. line.
                        if (lines[curr].Contains("الطالب"))
                        {
                            student_lines.Add(lines[curr]); //Save student's line to a variable.
                            students_sections.Add(0); //Add a new student with 0 sections.
                        }

                        //'المعدل التراكمي' is a unique word in the student's department line.
                        else if (lines[curr].Contains("المعدل التراكمي"))
                            student_dept_lines.Add(lines[curr]); //Save student's department line to a variable.

                        //'نظري' and 'عملي' are unique words in the section line. It must be one of them each line.
                        else if (lines[curr].Contains("نظري") || lines[curr].Contains("عملي"))
                        {
                            await CreateCourse(lines[curr]); //Create a new course.
                            await CreateInstructor(lines[curr]); //Create a new instructor.
                            section.Add(await CreateSection(lines[curr])); //Create a new section and add it to the list.
                            students_sections[students_sections.Count-1]++; //Add a section to the the last added student.
                        }
                    }
                }
                int counter = 0, i = 0; //Counters
                //Assign departments to students then add students.
                foreach (var line in student_lines.Zip(student_dept_lines, Tuple.Create))
                {
                    var dept = await CreateStudentDept(line.Item2);
                    var student = await CreateStudent(line.Item1, dept);
                    while (students_sections[i] > 0)
                    {
                        await CreateSchedule(section[counter++], student); //Add new section to student's schedule.
                        students_sections[i]--;
                    }
                    i++;
                }
            }
        }
        
        private async Task<Department> CreateStudentDept(string line)
        {
            var l = line.Split(",");
            var deptName = l[9].Trim();

            var dept = await _context.Department
                .FirstOrDefaultAsync(d => d.Name.Equals("علوم الحاسب"));

            return dept;
        }

        private async Task<Student> CreateStudent(string line, Department dept)
        {
            var l = line.Split(",");

            var student = new Student()
            {
                ID = l[10],
                FullName = l[12].Trim(),
                DepartmentCode = dept.Code
            };

            if (!_context.Student.Any(s => s.ID == l[10]))
            {
                _context.Student.Add(student);
                await _context.SaveChangesAsync();

                await CreateStudentUser(student);
            }

            return student;
        }

        private async Task CreateStudentUser(Student student)
        {
            if (!_context.User.Any(u => u.Username == student.ID))
            {
                var studentUser = new User()
                {
                    Username = student.ID,
                    Password = student.ID,
                    Type = "Student"
                };
                _context.User.Add(studentUser);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CreateCourse(string line)
        {
            var l = line.Split(",");
            string[] deptCode = l[1].Split(" ");

            if (!_context.Course.Any(co => co.Code == l[1]))
            {
                if (!_context.Department.Any(d => d.Code == deptCode[0]))
                    deptCode[0] = "Other";
                var course = new Course()
                {
                    Code = l[1],
                    Title = l[5].Trim(),
                    DepartmentCode = deptCode[0]
                };
                _context.Course.Add(course);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CreateInstructor(string line)
        {
            var l = line.Split(",");

            if (!_context.Instructor.Any(i => i.FullName == l[45].Trim()))
            {
                var instructor = new Instructor()
                {
                    FullName = l[45].Trim()
                };
                _context.Instructor.Add(instructor);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<Section> CreateSection(string line)
        {
            var l = line.Split(",");

            var instructor = await _context.Instructor
                  .FirstOrDefaultAsync(i => i.FullName.Equals(l[45].Trim()));

            var section = new Section()
            {
                Number = l[17],
                Activity = l[14].Trim(),
                Duration = l[19],
                Day = l[21].Trim(),
                StartTime = l[23],
                EndTime = l[26],
                FinalExam = l[40],
                CourseCode = l[1],
                InstructorID = instructor.ID
            };

            if (!_context.Section.Any(se => se.Number == l[17]))
            {
                _context.Section.Add(section);
                await _context.SaveChangesAsync();
            }

            return section;
        }

        private async Task CreateSchedule(Section section, Student student)
        {
            if (!_context.Schedule.Any(sc => sc.Student.ID == student.ID && sc.Section.Number == section.Number))
            {
                var schedule = new Schedule()
                {
                    StudentID = student.ID,
                    SectionNumber = section.Number
                };
                _context.Schedule.Add(schedule);
                await _context.SaveChangesAsync();
            }
        }

        public IActionResult UploadedFiles(List<IFormFile> csv_files)
        {
            var uploaded = new UploadViewModel()
            {
                Files = csv_files
            };

            return View(uploaded);
        }
    }
}

/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COCAS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;

namespace COCAS.Controllers
{
    public class FilesController : BaseController
    {
        private readonly COCASContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public FilesController(COCASContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        public IActionResult Upload()
        {
            //if (!IsStaff())
              //  return RedirectToAction("Login_Ar","Users");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            //if (!IsLoggedIn())
            //  return RedirectToAction("Login_Ar", "Users");
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Files");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(path, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                var csv_files = IsCSV(files);
                ReadCSVFiles(csv_files);
                
                //return RedirectToAction(nameof(UploadedFiles), csv_files);
            }

            return View();
        }
        
        private List<IFormFile> IsCSV(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var csv_files = new List<IFormFile>();

            foreach (var f in files)
            {
                if (f.FileName.EndsWith(".xlsx"))
                    if (f.Length > 0)
                        csv_files.Add(f);
            }

            return csv_files;
        }
        
        private void ReadCSVFiles(List<IFormFile> csv_files)
        {
            foreach (var f in csv_files)
            {
                var lines = new List<string>(); //Lines per file.
                var student_lines = new List<string>(); //Lines with students info.
                var student_dept_lines = new List<string>(); //Lines with students' departments info.
                var students_sections = new List<int>(); //Number of sections per students.
                var section = new List<Section>();
                XSSFWorkbook workBook;
                using (FileStream file = new FileStream(Path.Combine(_hostingEnvironment.WebRootPath, f.FileName), FileMode.Open, FileAccess.Read))
                {
                    workBook = new XSSFWorkbook(file);
                }
                StringBuilder output = new StringBuilder();
                XSSFSheet sheet = workBook.GetSheetAt(0) as XSSFSheet;
                output.AppendFormat("{0} ", sheet.GetRow(0));
                //ViewData["a"] = sheet.GetCellComment(0).StringCellValue;

                /*
                using (var reader = new StreamReader(f.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        lines.Add(reader.ReadLine());
                        var curr = lines.Count - 1;

                        //'الطالب' is a unique word in the student's info. line.
                        if (lines[curr].Contains("الطالب"))
                        {
                            student_lines.Add(lines[curr]); //Save student's line to a variable.
                            students_sections.Add(0); //Add a new student with 0 sections.
                        }

                        //'المعدل التراكمي' is a unique word in the student's department line.
                        else if (lines[curr].Contains("المعدل التراكمي"))
                            student_dept_lines.Add(lines[curr]); //Save student's department line to a variable.

                        //'نظري' and 'عملي' are unique words in the section line. It must be one of them each line.
                        else if (lines[curr].Contains("نظري") || lines[curr].Contains("عملي"))
                        {
                            await CreateCourse(lines[curr]); //Create a new course.
                            await CreateInstructor(lines[curr]); //Create a new instructor.
                            section.Add(await CreateSection(lines[curr])); //Create a new section and add it to the list.
                            students_sections[students_sections.Count-1]++; //Add a section to the the last added student.
                        }
                    }
                }
                int counter = 0, i = 0; //Counters
                //Assign departments to students then add students.
                foreach (var line in student_lines.Zip(student_dept_lines, Tuple.Create))
                {
                    var dept = await CreateStudentDept(line.Item2);
                    var student = await CreateStudent(line.Item1, dept);
                    while (students_sections[i] > 0)
                    {
                        await CreateSchedule(section[counter++], student); //Add new section to student's schedule.
                        students_sections[i]--;
                    }
                    i++;
                }
            }
        }
        
        private async Task<Department> CreateStudentDept(string line)
{
    var l = line.Split(",");
    var deptName = l[9].Trim();

    var dept = await _context.Department
        .FirstOrDefaultAsync(d => d.Name.Equals("علوم الحاسب"));

    return dept;
}

private async Task<Student> CreateStudent(string line, Department dept)
{
    var l = line.Split(",");

    var student = new Student()
    {
        ID = l[10],
        FullName = l[12].Trim(),
        DepartmentCode = dept.Code
    };

    if (!_context.Student.Any(s => s.ID == l[10]))
    {
        _context.Student.Add(student);
        await _context.SaveChangesAsync();

        await CreateStudentUser(student);
    }

    return student;
}

private async Task CreateStudentUser(Student student)
{
    if (!_context.User.Any(u => u.Username == student.ID))
    {
        var studentUser = new User()
        {
            Username = student.ID,
            Password = student.ID,
            Type = "Student"
        };
        _context.User.Add(studentUser);
        await _context.SaveChangesAsync();
    }
}

private async Task CreateCourse(string line)
{
    var l = line.Split(",");
    string[] deptCode = l[1].Split(" ");

    if (!_context.Course.Any(co => co.Code == l[1]))
    {
        if (!_context.Department.Any(d => d.Code == deptCode[0]))
            deptCode[0] = "Other";
        var course = new Course()
        {
            Code = l[1],
            Title = l[5].Trim(),
            DepartmentCode = deptCode[0]
        };
        _context.Course.Add(course);
        await _context.SaveChangesAsync();
    }
}

private async Task CreateInstructor(string line)
{
    var l = line.Split(",");

    if (!_context.Instructor.Any(i => i.FullName == l[45].Trim()))
    {
        var instructor = new Instructor()
        {
            FullName = l[45].Trim()
        };
        _context.Instructor.Add(instructor);
        await _context.SaveChangesAsync();
    }
}

private async Task<Section> CreateSection(string line)
{
    var l = line.Split(",");

    var instructor = await _context.Instructor
          .FirstOrDefaultAsync(i => i.FullName.Equals(l[45].Trim()));

    var section = new Section()
    {
        Number = l[17],
        Activity = l[14].Trim(),
        Duration = l[19],
        Day = l[21].Trim(),
        StartTime = l[23],
        EndTime = l[26],
        FinalExam = l[40],
        CourseCode = l[1],
        InstructorID = instructor.ID
    };

    if (!_context.Section.Any(se => se.Number == l[17]))
    {
        _context.Section.Add(section);
        await _context.SaveChangesAsync();
    }

    return section;
}

private async Task CreateSchedule(Section section, Student student)
{
    if (!_context.Schedule.Any(sc => sc.Student.ID == student.ID && sc.Section.Number == section.Number))
    {
        var schedule = new Schedule()
        {
            StudentID = student.ID,
            SectionNumber = section.Number
        };
        _context.Schedule.Add(schedule);
        await _context.SaveChangesAsync();
    }
}

public IActionResult UploadedFiles(List<IFormFile> csv_files)
{
    var uploaded = new UploadViewModel()
    {
        Files = csv_files
    };

    return View(uploaded);
}
    }
}
*/