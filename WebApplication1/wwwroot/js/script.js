///Console mesajı
console.log("Merhaba, sitemize hoşgeldin!");



const button = document.querySelector("#mainButton");
button.addEventListener("click", isCheck);

function isCheck() {
    var array=[];
    const isChecked = document.querySelectorAll("#flexCheckDefault");
    isChecked.forEach(element => {
        if (element.checked === true) {
            array.push(element.nextElementSibling.textContent);
        }
    });
    //document.write(array);
    alert(array);
    //return array;
}
//document.getElementById("demo").innerHTML = myFunction();
