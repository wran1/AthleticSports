$(function () {
    $('.radio-inline input').each(function (index, item) {
    }).click(function () {
        $(this).attr("checked", "checked");
    });
    $('.checkbox-inline input').each(function (index, item) {
    }).click(function () {
        $(this).attr("checked", "checked");
    });
    $('.input-group input[type=text]').each(function (index, item) {
    }).focusout(function () {
        var txtvalue = $(this).val();
        $(this).attr("value", txtvalue);
    });
});
function submitResearchForm() {
    validate();
    if (window.returnvalue) {
        var obj = document.getElementById("researchFormDetail").innerHTML;
        var title = $('#researchtitle').text();
        $.post("/Research/SendResearchEmail", {
            researchhtml: obj, title: title
        }, function (data, status) {
            if (data != null) {
                if (data == true) {
                    alert("提交成功！");
                } else {
                    alert(data);
                }
            }
        });
    }

}
function validate() {
    window.returnvalue = true;
    var el = document.getElementById("researchFormDetail").getElementsByTagName('input');
    for (var i = 0; i < el.length; i++) {
        if (el[i].type == 'radio') {
            var radiogroup = document.getElementsByName(el[i].name);
            var itemchecked = false;
            for (var j = 0; j < radiogroup.length; ++j) {
                if (radiogroup[j].checked) {
                    itemchecked = true;
                    break;
                }
            }
            if (!itemchecked) {
                alert("单选题没做完");
                window.returnvalue = false;
                return false;

            }
        }
        else if (el[i].type == 'checkbox') {
            var itemchecked = false;
            var elems = document.getElementsByTagName("input");
            for (var j = 0; j < elems.length; j++) {
                if (elems[j].type == 'checkbox' && elems[j].name == el[i].name) {
                    if (elems[j].checked) {
                        itemchecked = true;
                        break;
                    }
                }
            }
            if (!itemchecked) {
                alert("多选题没做完");
                window.returnvalue = false;
                return false;
            }
        }
        else if (el[i].type == 'text') {
            var itemchecked = true;
            var obj = document.getElementById("diaocha4text");
            if (obj != null) {
                var elems = document.getElementById("diaocha4text").getElementsByTagName("input");
                //var elems = document.getElementsByTagName("input");
                for (var j = 0; j < elems.length; j++) {
                    if (elems[j].type == 'text') {
                        if (elems[j].value == "") {
                            itemchecked = false;
                            break;
                        }
                    }
                }
                if (!itemchecked) {
                    alert("联系人及联系方式未填全");
                    window.returnvalue = false;
                    return false;
                }
            }
            //itemchecked = false;
        }
        //return itemchecked;
    }
    var txtarealist = document.getElementsByTagName('textarea');
    if (txtarealist != null && txtarealist.length > 0) {
        for (var i = 0; i < txtarealist.length; i++) {
            var itemchecked = true;
            var txtval = $(txtarealist[i]).val();
            if (txtval == "") {
                itemchecked = false;
                break;
            }
        }
        if (!itemchecked) {
            alert("问卷问题未填全");
            window.returnvalue = false;
            return false;
        }
    }
}