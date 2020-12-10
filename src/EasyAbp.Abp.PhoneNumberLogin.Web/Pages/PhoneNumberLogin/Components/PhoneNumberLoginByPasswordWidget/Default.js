(function ($) {

    $(document).ready(function () {

        abp.widgets.PhoneNumberLoginByPassword = function ($widget) {
            
            var widgetManager = $widget.data('abp-widget-manager');

            function getFilters() {
                return { };
            }

            function init() {
            }
            
            return {
                init: init,
                getFilters: getFilters
            };
        };

        var widgetManager = new abp.WidgetManager({filterForm: 'PhoneNumberLoginByPassword'});
        widgetManager.init();
    });

    $("form").submit(function (e) {
        e.preventDefault();

        $(this).ajaxSubmit({
            type: 'post',
            url: '/api/phone-number-login/account/login/by-password',
            success: function(data) {
                console.log(data)
                if (data.result === 1) {
                    var urlParams = new URLSearchParams(document.location.search.slice(1));
                    var returnUrl = urlParams.get('ReturnUrl');
                    var returnUrlHash = urlParams.get('ReturnUrlHash');
                    var targetUrl = document.location.origin;
                    if (returnUrl) targetUrl += decodeURI(returnUrl);
                    if (returnUrlHash) targetUrl += returnUrlHash;
                    document.location.href = targetUrl;
                } else {
                    abp.message.error(abp.localization.localize(data.description))
                }
            },
        })
    });

})(jQuery);