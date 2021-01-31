//bootbox.setLocale('br');
function GetTranslation(key_en) {
    if (_DATA_translations[key_en])
        return _DATA_translations[key_en];
    $.get('/API/SaveTranslation', { key_en: key_en });
    return key_en;
}
$(".deleter").click(function (e) {
    var _this = this;
    e.preventDefault();
    bootbox.confirm(GetTranslation("Do you really wish to remove this register?"), function (result) {
        if (result) {
            // @ts-ignore
            document.location = $(_this).attr('href');
        }
    });
});
// @ts-ignore
$('[data-toggle="tooltip"]').tooltip();
// @ts-ignore
$('[tooltip]').tooltip({
    title: function () {
        return $(this).attr('tooltip');
    }
});
//# sourceMappingURL=global.js.map