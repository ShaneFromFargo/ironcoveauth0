/*Logic to modify html to order pizza*/

$(document).ready(function () {

    //Dont allow users to order pizza if they are logged in
    if (!_loggedIn) {
        $(".pizza-add").hide();
    }

    $(".order-submit").hide();

    //Update order
    $(".pizza-add").click(function () {
        var itemName = $(this).parent().find("h3").text();
        var price = $(this).find(".price").text();
        $("#shopping-table-body").append("<tr><td>" + itemName + "</td><td>" + price + "</td><td><button class=\"btn btn-xs btn-danger remove-from-cart\">Remove</button></td></tr>");
        updatePrice();
    });

    //Remove unwanted items
    $("#shopping-table").on("click", ".remove-from-cart", function () {
        $(this).parent().parent().remove();
        updatePrice();
    });

    //Place order
    $(".order-submit-logged-in").click(function () {
        alert("Order succesfully placed!");
    });
});

//Function used to calculate total order price
function updatePrice() {
    var priceNumber = 0;

    //Find each item price
    $('#shopping-table > tbody > tr').each(function () {
        var price = $(':nth-child(2)', this).text().substr(1);
        //add it to the total
        priceNumber = parseFloat(price) + priceNumber;
    });
    if (priceNumber != 0) {

        $(".order-submit").show();
    }
    else {
        //dont let users submit orders with 0 total
        $(".order-submit").hide();
    }
    $("#total").text(priceNumber.toFixed(2));
}