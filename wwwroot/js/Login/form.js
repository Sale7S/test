$(function () {
    var nonEmpty = "non-empty";
    var inputs = jQuery('.input input');
    var setLabelStyle = function setLabelStyle() {
    var label = jQuery(this);

    if (label.val().length) {
        label.addClass(nonEmpty);
    } else {
        label.removeClass(nonEmpty);
    }
};
    inputs.focus(function () {jQuery(this).addClass(nonEmpty); });
    inputs.blur(setLabelStyle);
    inputs.each(setLabelStyle);
});