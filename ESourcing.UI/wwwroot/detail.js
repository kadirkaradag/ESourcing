﻿//var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:17962/auctionHub").build();  ocelot gateway den önce
var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:8001/auctionHub").build(); // ocelot gateway ile

var auctionId = document.getElementById("AuctionId").value; // Detail.cshtml icindeki hiddenFor aracılığı ile alıyoruz.

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

//var id = document.getElementById("AuctionId").value;
var groupName = "auction-" + auctionId;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("AddToGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("Bids", function (user, bid) {
    addBidToTable(user, bid);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("SellerUserName").value; // Detail.cshtml icindeki hiddenFor aracılığı ile alıyoruz.
    var productId = document.getElementById("ProductId").value; // Detail.cshtml icindeki hiddenFor aracılığı ile alıyoruz.
    var sellerUser = user;
    var bid = document.getElementById("exampleInputPrice").value;

    var sendBidRequest = {
        AuctionId: auctionId,
        ProductId: productId,
        SellerUserName: sellerUser,
        Price: parseFloat(bid).toString()
    }

    SendBid(sendBidRequest);

    event.preventDefault();
});

document.getElementById("finishButton").addEventListener("click", function (event) {
    SendCompleteAuction();

    event.preventDefault();
});

function addBidToTable(user, bid) {
    var str = "<tr>";
    str += "<td>" + user + "</td>";
    str += "<td>" + bid + "</td>";
    str += "</tr>";

    if ($('table > tbody > tr:first').length > 0) {
        $('table > tbody > tr:first').before(str);
    }
    else {
        $('.bidLine').append(str);
    }
}

function SendBid(model) {

    $.ajax({
        url: "/Auction/SendBid",
        type: "POST",
        data: model,
        success: function (response) {
            if (response.isSuccess) {
                document.getElementById("exampleInputPrice").value = "";

                connection.invoke("SendBidAsync", groupName, model.SellerUserName, model.Price).catch(function (err) {
                    return console.error(err.toString());
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });

}

function SendCompleteAuction() {

    var id = auctionId;
    $.ajax({
        url: "/Auction/CompleteAuction",
        type: "POST",
        data: {id:id},
        success: function () {
                console.log("İşleminiz Başarıyla Sonuçlandı");
                window.location.href = '/Auction';            
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });

}