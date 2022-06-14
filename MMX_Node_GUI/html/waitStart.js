
function checkNodeStart() {
    fetch('/wapi/node/').then(
        (response) => response.text()
    ).then((data) => {
        var check = data.indexOf('node/info') >=0
        if (check) {
            window.location = '/gui/';
        } else {
            throw new Error('Something went wrong');
        }
    }).catch((error) => {
        console.log(error)
        setTimeout(() => checkNodeStart(), 500);
    })
}

checkNodeStart();