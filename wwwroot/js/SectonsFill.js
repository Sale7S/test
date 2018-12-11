$(function () {
    var nonEmpty = "non-empty";
    var input = jQuery('input #@Model.SectionsNumbers[i]');
    var label1 = jQuery('label #CourseCode');
    var label2 = jQuery('label #CourseTitle');
    var label3 = jQuery('label #SectionDuration');
    document.getElementById("Fill").addEventListener("click", function () {
        //if ()
    });
    var setLabel = function setLabel() {

        if (label1.val().length) {
            label1.addClass(nonEmpty);
        } else {
            label1.removeClass(nonEmpty);
        }
    };
    inputs.focus(function () { label1.addClass(nonEmpty); });
    inputs.blur(setLabelStyle);
});