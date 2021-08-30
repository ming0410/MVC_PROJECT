
//確認是否為行動裝置，含平板
function checkMobile() {
    var mobiles = new Array
        (
        "midp", "j2me", "avant", "docomo", "novarra", "palmos", "palmsource",
        "240x320", "opwv", "chtml", "pda", "windows ce", "mmp/",
        "blackberry", "mib/", "symbian", "wireless", "nokia", "hand", "mobi",
        "phone", "cdm", "up.b", "audio", "sie-", "sec-", "samsung", "htc",
        "mot-", "mitsu", "sagem", "sony", "alcatel", "lg", "eric", "vx",
        "NEC", "philips", "mmm", "xx", "panasonic", "sharp", "wap", "sch",
        "rover", "pocket", "benq", "java", "pt", "pg", "vox", "amoi",
        "bird", "compal", "kg", "voda", "sany", "kdd", "dbt", "sendo",
        "sgh", "gradi", "jb", "dddi", "moto", "iphone", "android",
        "iPod", "incognito", "webmate", "dream", "cupcake", "webos",
        "s8000", "bada", "googlebot-mobile"
        )
    var ua = navigator.userAgent.toLowerCase();
    var isMobile = false;
    for (var i = 0; i < mobiles.length; i++) {
        if (ua.indexOf(mobiles[i]) > 0) {
            isMobile = true;
            break;
        }
    }
    return isMobile;
}

//確認是否為手機非平板
function checkPhone() {
    return jQuery.browser.mobile;
}

//顯示目前時間及周別
function ShowTime() {
    var today = new Date();
    var yy = today.getFullYear();
    var mm = today.getMonth() + 1;  // getMonth() is zero-based
    var dd = today.getDate();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    var w = getYearWeek(today);
    //日期時間固定兩碼
    mm = checkTime(mm);
    dd = checkTime(dd);
    m = checkTime(m);
    s = checkTime(s);
    w = checkTime(w);

    //手機不要顯示時間
    if (checkPhone()) {
        $('#nav_li_txtTime').css('display', 'none');        
    }
    //行動裝置只顯示時間
    else if (checkMobile()) {
        $("#nav_txtTime").html(h + ":" + m + ":" + s);
    }
    else {
        document.getElementById('nav_txtTime').innerHTML = yy + "/" + dd + "/" + dd + " " + h + ":" + m + ":" + s;
    }        
    t = setTimeout('ShowTime()', 500);
}

function HeadNavbarStyle() {
    if (checkMobile()) {
        //行動裝置將導覽列之使用者頭像隱藏
        $('#nav_li_UserMenu').css('display', 'none');
    }
    
    //在導覽列顯示現在日期/時間/周別
    ShowTime();
}

//日期時間固定兩碼，add a zero in front of numbers < 10
function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

//取得周別
function getYearWeek(date) {
    var date2 = new Date(date.getFullYear(), 0, 1);
    var day1 = date.getDay();
    if (day1 == 0) day1 = 7;
    var day2 = date2.getDay();
    if (day2 == 0) day2 = 7;
    d = Math.round((date.getTime() - date2.getTime() + (day2 - day1) * (24 * 60 * 60 * 1000)) / 86400000);
    return Math.ceil(d / 7) + 1;
}

//顯示大小
function ShowSize() {
    //clientHeight: only padding; offsetHeight: includes padding, scrollBar and borders
    var offsetHeight = document.getElementById('div_content').offsetHeight;
    document.getElementById('nav_txtSize').innerHTML = "Height:" + offsetHeight;
}


function keydownhandle(e) {
    if (e.which == 13) {
        alert("KeyDownEvent");
        e.preventDefault(); //stops default action: submitting form
        $(this).blur();
        $('#btnQuery').focus().click();//give your submit an ID
    }
}

