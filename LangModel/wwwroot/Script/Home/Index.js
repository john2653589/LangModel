

$('#WelcomeDiv').text(Lang.Get('WelcomeDiv'));
$('#BtnChange').text(Lang.Get('BtnChange'));

function ChangeLang() {
    let SetLang = '.AspNetCore.Culture=';
    if (document.cookie.includes('zh-tw'))
        SetLang += 'en-us';
    else
        SetLang += 'zh-tw';

    document.cookie = SetLang;
    location.reload();
}