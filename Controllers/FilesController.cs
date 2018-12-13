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
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace COCAS.Controllers
{
    public class FilesController : BaseController
    {
        private readonly COCASContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _xlsxPath;

        public FilesController(COCASContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
            _xlsxPath = Path.Combine(environment.WebRootPath, @"Files\xlsx");
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
                var xlsxFiles = await IsXLSX(files);
                await ReadXLSXFiles(xlsxFiles);

                ViewData["Done"] = "تم الرفع";
            }

            return View();
        }
        
        private async Task<List<IFormFile>> IsXLSX(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var xlsxFiles = new List<IFormFile>();

            foreach (var f in files)
            {
                if (f.FileName.EndsWith(".xlsx"))
                    if (f.Length > 0)
                    {
                        xlsxFiles.Add(f);

                        var filePath = Path.Combine(_xlsxPath, f.FileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await f.CopyToAsync(fileStream);
                        }
                    }
            }

            return xlsxFiles;
        }

        private async Task ReadXLSXFiles(List<IFormFile> csv_files)
        {
            foreach (var f in csv_files)
            {
                var lines = new List<IRow>(); //Lines per file.
                var student_lines = new List<IRow>(); //Lines with students info.
                var student_dept_lines = new List<IRow>(); //Lines with students' departments info.
                var students_sections = new List<int>(); //Number of sections per students.
                var section = new List<Section>();

                var filePath = Path.Combine(_xlsxPath, f.FileName);
                XSSFWorkbook workBook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workBook = new XSSFWorkbook(file);
                }

                ISheet sheet = workBook.GetSheetAt(0) as XSSFSheet;
                for (int row = 0; row <= sheet.LastRowNum; row++)
                {
                    if (sheet.GetRow(row) != null)
                    {
                        lines.Add(sheet.GetRow(row));
                        var curr = lines.Count - 1;

                        var cells = lines[curr].Cells;

                        foreach (var c in cells)
                        {
                            DataFormatter formatter = new DataFormatter();
                            String cell = formatter.FormatCellValue(c);

                            if (cell.Contains("الطالب"))
                            {
                                student_lines.Add(lines[curr]); //Save student's line to a variable.
                                students_sections.Add(0); //Add a new student with 0 sections.
                            }
                            else if (cell.Contains("المعدل التراكمي"))
                                student_dept_lines.Add(lines[curr]); //Save student's department line to a variable.

                            //'نظري' and 'عملي' are unique words in the section line. It must be one of them each line.
                            else if (cell.Contains("نظري") || cell.Contains("عملي"))
                            {
                                section.Add(await CreateSection(lines[curr])); //Create a new section and add it to the list. It creates courses and instructors too.
                                students_sections[students_sections.Count - 1]++; //Add a section to the the last added student.
                            }

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
        
        private async Task<Department> CreateStudentDept(IRow line)
        {
            var deptName = line.GetCell(9).StringCellValue.Trim();

            var dept = await _context.Department
                .FirstOrDefaultAsync(d => d.Name.Equals("علوم الحاسب"));

            return dept;
        }

        private async Task<Student> CreateStudent(IRow line, Department dept)
        {
            var student = new Student()
            {
                ID = line.GetCell(10).NumericCellValue.ToString(),
                FullName = line.GetCell(12).StringCellValue.Trim(),
                DepartmentCode = dept.Code
            };

            if (!_context.Student.Any(s => s.ID == line.GetCell(10).NumericCellValue.ToString()))
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

        private async Task<string> CreateCourse(IRow line)
        {
            string[] deptCode = line.GetCell(1).StringCellValue.Split(" ");

            if (!_context.Course.Any(co => co.Code == line.GetCell(1).StringCellValue))
            {
                if (!_context.Department.Any(d => d.Code == deptCode[0]))
                    deptCode[0] = "Other";
                var course = new Course()
                {
                    Code = line.GetCell(1).StringCellValue,
                    Title = line.GetCell(5).StringCellValue.Trim(),
                    DepartmentCode = deptCode[0]
                };
                _context.Course.Add(course);
                await _context.SaveChangesAsync();
            }

            return line.GetCell(1).StringCellValue;
        }

        private async Task<int> CreateInstructor(IRow line)
        {
            string fullName = line.GetCell(45).StringCellValue.Trim();
            
            if (!_context.Instructor.Any(i => i.FullName == fullName))
            {
                var instructor = new Instructor()
                {
                    FullName = fullName
                };
                _context.Instructor.Add(instructor);
                await _context.SaveChangesAsync();
            }
            var inst = await _context.Instructor
                .FirstOrDefaultAsync(i => i.FullName == fullName);

            return inst.ID;
        }

        private async Task<Section> CreateSection(IRow line)
        {
            var courseCode = await CreateCourse(line);
            var instructorID = await CreateInstructor(line);
            
            DataFormatter formatter = new DataFormatter();
            String duration = formatter.FormatCellValue(line.GetCell(19));
            String day = formatter.FormatCellValue(line.GetCell(21));

            var section = new Section()
            {
                Number = line.GetCell(17).NumericCellValue.ToString(),
                Activity = line.GetCell(14).StringCellValue.Trim(),
                Duration = duration,
                Day = day,
                StartTime = line.GetCell(23).StringCellValue,
                EndTime = line.GetCell(26).StringCellValue,
                FinalExam = line.GetCell(40).StringCellValue,
                CourseCode = courseCode,
                InstructorID = instructorID
            };

            if (!_context.Section.Any(se => se.Number == line.GetCell(17).NumericCellValue.ToString()))
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
    }
}