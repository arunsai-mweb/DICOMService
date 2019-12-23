





(function ($) {
	$.dialog = function (title, body, attr, onShow, onHide, actionFn) {

		$('#modalDialog').dialog(title, body, attr, actionFn);

		$('#modalDialog').on('shown.bs.modal', function (e) {

			if (onShow && typeof onShow == "function") {

				onShow();
				//$(".modal-dialog").drags();
			}

		});


		$('#modalDialog').on('hidden.bs.modal', function (e) {
			var _that = this;

			if (onHide && typeof onHide == "function") {

				onHide();
			}
			$(_that).find('.modal-body,.modal-title').empty();

			$(_that).find('.modal-footer').find("button").removeAttr("onclick").unbind("bind");

			$(_that).off("hide.bs.modal");

		})
	};
	$.fn.dialog = function (title, body, attr, actionFn) {

		var that = this;

		var _div = $('<div/>').html(body);

		var _title = $(_div).find(".modal-title");

		if (_title.length > 0) {

			$(that).find('.modal-title').replaceWith(_title);

		} else {

			$(that).find('.modal-title').html(title);
		}

		var _footer = $(_div).find(".modal-footer");

		if (_footer.length > 0) {

			if (_footer.length > 1) {
				_footer.not(":first").remove();
			}

			$(that).find('.modal-footer').replaceWith(_footer);

		};

		$(that).find('.modal-body').empty().append(_div);

		$(that).find('.modal-body').find(".modal-title,.modal-footer").remove();

		$(that).modal(attr || { keyboard: false, show: true });

		if (actionFn && typeof actionFn == "function") {

			$(that).find(".btn-action").click(actionFn);
		}


		var _frm = $(that).find("form");

		_frm.removeData("validator");

		_frm.removeData("unobtrusiveValidation");

		$.validator.unobtrusive.parse(_frm);

	};


	$.showDialog = function (title, body, attr, onShow, onHide, actionFn) {

		$('#modalDialog').dialog(title, body, attr, actionFn);

		$('#modalDialog').on('shown.bs.modal', function (e) {

			if (onShow && typeof onShow == "function") {

				onShow();
				//$(".modal-dialog").drags();
			}

		});


		$('#modalDialog').on('hidden.bs.modal', function (e) {
			var _that = this;

			if (onHide && typeof onHide == "function") {

				onHide();
			}
			$(_that).find('.modal-body,.modal-title').empty();

			$(_that).find('.modal-footer').find("button").removeAttr("onclick").unbind("bind");

			$(_that).off("hide.bs.modal");
			window.location.reload();
		})
	};
	$.fn.showDialog = function (title, body, attr, actionFn) {

		var that = this;

		var _div = $('<div/>').html(body);

		var _title = $(_div).find(".modal-title");

		if (_title.length > 0) {

			$(that).find('.modal-title').replaceWith(_title);

		} else {

			$(that).find('.modal-title').html(title);
		}

		var _footer = $(_div).find(".modal-footer");

		if (_footer.length > 0) {

			if (_footer.length > 1) {
				_footer.not(":first").remove();
			}

			$(that).find('.modal-footer').replaceWith(_footer);

		};

		$(that).find('.modal-body').empty().append(_div);

		$(that).find('.modal-body').find(".modal-title,.modal-footer").remove();

		$(that).modal(attr || { keyboard: false, show: true });

		if (actionFn && typeof actionFn == "function") {

			$(that).find(".btn-action").click(actionFn);
		}


		var _frm = $(that).find("form");

		_frm.removeData("validator");

		_frm.removeData("unobtrusiveValidation");

		$.validator.unobtrusive.parse(_frm);

	}

})(jQuery);
