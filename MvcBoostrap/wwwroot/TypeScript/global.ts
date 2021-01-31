declare var _DATA_translations: object;
declare var _DATA_dbg: boolean;

//bootbox.setLocale('br');

function GetTranslation(key_en): string {
	if(_DATA_translations[key_en])
		return _DATA_translations[key_en];

	$.get('/API/SaveTranslation', { key_en });
	return key_en;
}

$(".deleter").click(function(e) {
	e.preventDefault();
	bootbox.confirm(GetTranslation("Do you really wish to remove this register?"), (result) => {
		if(result)
		{
			// @ts-ignore
			document.location = $(this).attr('href');
		}
	});
});

// @ts-ignore
$('[data-toggle="tooltip"]').tooltip();

// @ts-ignore
$('[tooltip]').tooltip({
	title: function() {
		return $(this).attr('tooltip');
	}
});
