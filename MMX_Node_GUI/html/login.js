var i = 0;
var login = (interval) => {
    fetch('/server/login?user=mmx-admin&passwd_plain=' + '_mmx_password_')
        .then(response => {
            clearInterval(interval);
            window.location = "/gui/";
        }).catch(error => {
        });

    if (i > 10) {
        clearInterval(interval);
        window.location = "/gui/";
    }
    i++;
}
var interval = setInterval(() => { login(interval) }, 1000);