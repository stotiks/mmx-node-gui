
function checkNodeStart() {
    fetch('/api/node/').then((response) => {
        if (response.ok) {
            return response.json();
        }
        throw new Error('Something went wrong');
    }).then((responseJson) => {
        var check = responseJson.includes('get_height')
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