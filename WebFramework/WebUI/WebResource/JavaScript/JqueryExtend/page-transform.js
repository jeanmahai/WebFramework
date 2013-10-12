(function (ns) {

    if (!window[ns]) window[ns] = {};

    function transform(context, html, effect) {
        if (!effect || effect === "") {
            context.html(html);
        }
        else if (effect === "fade") {
            context.animate({
                opacity: 0
            },{
				complete:function(){
					context.empty();
					context.append(html);
					context.animate({ opacity: 1 });
				}
			});
        }
    }

    function getPage(obj, effect) {
        if (obj.url === "") throw "请提供页面的url地址";
        obj.type = "GET";
        obj.dataType = "HTML";
        obj.context = $(document.body);
        obj.success = function (html) {
            transform(this, html, effect);
        };
        if (!obj.error) {
            obj.error = function (xhr, status, error) {
                if (console) {
                    console.error("status:" + status + ";error:" + error);
                }
            };
        }
        $.ajax(obj);
    };

    function initTagA() {
        $("a[data-transform]").each(function () {
            $(this).click(function () {
                var me = $(this);
                var href = me.attr("href");
                var effect = me.attr("data-transform");
                getPage({
                    url: href
                }, effect);
                return false;
            });
        });
    }

    $(function () {
        initTagA();
    });

})("PageTransform");