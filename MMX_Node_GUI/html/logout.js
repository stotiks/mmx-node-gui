var i = 0;
var login = (interval) => {
    fetch('/wapi/node/exit')
        .then(response => {
            clearInterval(interval);
        }).catch(error => {
        });

    if (i > 10) {
        clearInterval(interval);
    }
    i++;
}
var interval = setInterval(() => { login(interval) }, 1000);