//定义全局数据获取地址
var getIndestryDatsUrl = "/GetDatas/Industrys";
var getDExhibitionDatsUrl = "/GetDatas/DomesticExhibitions";
var getOExhibitionDatsUrl = "/GetDatas/OverseasExhibitions";
//初始化级联选择器函数
function InitialSelectors(getDatasUrl, str4id, fieldname, CurrentCode) {
    InitialSelector(getDatasUrl, str4id, fieldname, 1, CurrentCode);
    if (CurrentCode.length >= 3) {
        InitialSelector(getDatasUrl, str4id, fieldname, 2, CurrentCode);
    }
    $(str4id + fieldname + "1").change(function() {
        SelectorChange(getDatasUrl, str4id, fieldname, 1);
    });
    $(str4id + fieldname + "2").change(function() {
        SelectorChange(getDatasUrl, str4id, fieldname, 2);
    });
}

function InitialSelector(getDatasUrl, str4id, fieldname,index, CurrentCode) {
    var selector = str4id + fieldname + index;
    var parentSystemId = CurrentCode.substring(0, (index - 1) * 3);
    var currentSystemId = CurrentCode.substring(0, index * 3);
    $.getJSON(getDatasUrl, { SystemId: parentSystemId }, function (data, status) {
        if (data.length > 0) {
            $(selector).append("<option value=''>请选择</option>")
            $.each(data, function (i, item) {
                if (item.SystemId == currentSystemId) {
                    $(selector).append("<option Selected value='" + item.SystemId + "'>" + item.Name + "</option>")
                } else {
                    $(selector).append("<option value='" + item.SystemId + "'>" + item.Name + "</option>")
                }
            });
            $(selector).show();
        }
    });
}

function SelectorChange(getDatasUrl, str4id, fieldname, index) {
    var selector = str4id + fieldname + index;
    var selectedCode = $(selector).val();
    if (selectedCode == '')
        return;
    $(str4id + fieldname).val(selectedCode);
    if (index < 3) {
        var slectorChild = str4id + fieldname + (index + 1);
        $.getJSON(getDatasUrl, { SystemId: selectedCode }, function (data, status) {
            if (data.length > 0) {
                $(slectorChild).empty();
                $(slectorChild).append("<option value=''>请选择</option>")
                $.each(data, function (i, item) {
                    $(slectorChild).append("<option value='" + item.SystemId + "'>" + item.Name + "</option>")
                });
                $(slectorChild).show();
            } else{
                $(slectorChild).hide();
            }
        });
}
}

