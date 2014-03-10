/*
用于兼容html5的新特性placeholder
@html 是html代码片段,如果为null,则默认是document
需要外部css的支持
.placeholder{} 用于缺省文本的样式
*/

function initPlacholder(html) {
    if ("placeholder" in document.createElement("input")) return;

    var css = "placeholder";

    var texts
        ,passwords;
    var textFilter = "input[type=text],area"
        ,passwordFilter="input[type=password]";
    if(html) {
        texts = $(html).find(textFilter);
        passwords = $(html).find(passwordFilter);
    }
    else {
        texts = $(textFilter);
        passwords = $(passwordFilter);
    }
    
    if(texts) {
        texts.each(function () {
            var pl = $(this).attr("placeholder");
            if (pl) {
                $(this).val(pl).addClass(css);
            }
        });
        texts.bind("focus", function () {
            var val = $.trim($(this).val());
            var pl = $(this).attr("placeholder");
            if (val == pl) {
                $(this).val("").removeClass("placeholder");
            }
        });
        texts.bind("blur", function () {
            var val = $.trim($(this).val());
            var pl = $(this).attr("placeholder");
            if (val == "") {
                $(this).val(pl);
                $(this).addClass("placeholder");
            }
        });
    }
    if(passwords) {
        passwords.each(function () {
            var pl = $(this).attr("placeholder");
            if (pl) {
                $(this).val(pl).addClass(css);
            }
        });
        passwords.bind("focus", function () {
            var val = $.trim($(this).val());
            var pl = $(this).attr("placeholder");
            if (val == pl) {
                $(this).val("").removeClass("placeholder");
                $(this).attr("type", "password");
            }
        });
        passwords.bind("blur", function () {
            var val = $.trim($(this).val());
            var pl = $(this).attr("placeholder");
            if (val == "") {
                $(this).val(pl);
                $(this).addClass("placeholder");
                $(this).attr("type", "text");
            }
        });
    }
}