(function (ns) {

    if (!window[ns]) window[ns] = {};

    function transform(context, html, effect) {
        if (!effect || effect === "") {
            context.html(html);
        }
        else if(effect==="FadeInFadeOut") {
            fadeInFadeOut(context,html);
        }
		else if(effect==="FadeInSlideRight"){
			fadeInSlide(context,html,"right");
		}
		else if(effect==="FadeInSlideLeft"){
			fadeInSlide(context,html,"left");
		}
    }
	
	function fadeInFadeOut(context, html){
		context.animate({
			opacity: 0
		},{
			complete:function(){
				context.html(html);
				context.animate({ opacity: 1 });
			}
		});
	}
	
	function fadeInSlide(context,html,direction){
		context.animate({
			opacity:0
		},{
			complete:function(){
				$("html").css({overflow:"hidden"});
				var winWidth=$(window).width();
				context.css({position:"relative",left:-winWidth,opacity:1});
				if(direction==="left") context.css({left:winWidth});
				if(direction==="right") context.css({left:-winWidth});
				context.html(html);
				context.animate({
					left:0
				},{
					complete:function(){
						$("html").css("overflow","");
						context.css("position","");
					}
				});
			}
		});
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