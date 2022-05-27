var login = () => {
    fetch('/server/login?user=mmx-admin&passwd_plain=_mmx_password_')
        .then(response => {
            window.location = "/gui/";
        }).catch(error => {
        });
}
login();
var interval = setInterval(() => { login() }, 500);