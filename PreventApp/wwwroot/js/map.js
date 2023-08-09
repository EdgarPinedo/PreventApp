
var mapOptions = {
    center: [20.6696, -103.3405],
    zoom: 13,
    minZoom:12,
    zoomControl: false,
    maxBounds: [
        //Suroeste
        [20.334647543695734, -103.00506591796875],
        //Noreste
        [20.90500300215911, -103.74320983886719]
    ]
}

var map = L.map('map', mapOptions);
L.control.zoom({ position: "bottomright" }).addTo(map);

// Versión del mapa gratuita sin límite
L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

var alertMarker;
var alertForm = document.getElementById("alert-container");
var onMap = 0;

map.on('click', function (e) {
    if (alertForm.classList.contains("active")) {
        document.getElementById("latitud").value = e.latlng.lat;
        document.getElementById("longitud").value = e.latlng.lng;

        if (onMap == 1) {
            alertMarker.setLatLng(e.latlng);
        }
        else {
            alertMarker = new L.marker(e.latlng, { draggable: true }).addTo(map);
            onMap = 1;
        }

        alertMarker.on("dragend", function (e) {
            var latLng = e.target.getLatLng();
            document.getElementById("latitud").value = latLng.lat;
            document.getElementById("longitud").value = latLng.lng;
        });
    }
});

function removeMarker() {
    if (onMap == 1) {
        map.removeLayer(alertMarker);
        onMap = 0;
        document.getElementById("latitud").value = "";
        document.getElementById("longitud").value = "";
    }
}

var categorias = {
    1: "Accidente de tráfico",
    2: "Incendio",
    3: "Robo"
};

var icons = {
    1: '<i class="bi bi-car-front-fill"></i>',
    2: '<i class="bi bi-fire"></i>',
    3: '<svg xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 448 512"> <path d="M224 256c-57.2 0-105.6-37.5-122-89.3c-1.1 1.3-2.2 2.6-3.5 3.8c-15.8 15.8-38.8 20.7-53.6 22.1c-8.1 .8-14.6-5.7-13.8-13.8c1.4-14.7 6.3-37.8 22.1-53.6c5.8-5.8 12.6-10.1 19.6-13.4c-7-3.2-13.8-7.6-19.6-13.4C37.4 82.7 32.6 59.7 31.1 44.9c-.8-8.1 5.7-14.6 13.8-13.8c14.7 1.4 37.8 6.3 53.6 22.1c4.8 4.8 8.7 10.4 11.7 16.1C131.4 28.2 174.4 0 224 0c70.7 0 128 57.3 128 128s-57.3 128-128 128zM0 482.3C0 399.5 56.4 330 132.8 309.9c6-1.6 12.2 .9 15.9 5.8l62.5 83.3c6.4 8.5 19.2 8.5 25.6 0l62.5-83.3c3.7-4.9 9.9-7.4 15.9-5.8C391.6 330 448 399.5 448 482.3c0 16.4-13.3 29.7-29.7 29.7H29.7C13.3 512 0 498.7 0 482.3zM160 96c-8.8 0-16 7.2-16 16s7.2 16 16 16H288c8.8 0 16-7.2 16-16s-7.2-16-16-16H160z" /></svg>'
};

var markers = {
    1: 'images/trafico-icon.png',
    2: 'images/fire-icon.png',
    3: 'images/robo-icon.png'
};

model.forEach((accident) => {
    var index = accident.categoriaId;
    var date = new Date(accident.fecha);
    var diff = Math.abs(now.getTime() - date.getTime());
    var hours = Math.floor(diff / 3600000);
    var minutes;
    var timeInfo = "Hace ";

    if (hours == 0) {
        minutes = Math.floor((diff % 3600000) / 60000);
        timeInfo += minutes + " min";
    } else {
        timeInfo += hours + " h";
    }

    var popup = L.popup({ className: "categoria" + index, closeButton: false })
        .setContent("<div class='popup-title'>" + icons[index] + "<b class='b1'>" + categorias[index] + "</b><b class='b2'>" + timeInfo + "</b></div><hr/>" + accident.descripcion);

    var iconOptions = {
        iconUrl: markers[index],
        iconSize: [45, 45],
    };
    var customIcon = L.icon(iconOptions);

    L.marker([accident.latitud, accident.longitud], { icon: customIcon }).bindPopup(popup).addTo(map);
});

var span = document.getElementById("span");

if (model.length == 0) {
    span.style.display = 'block';
}

var tabs = document.querySelectorAll(".tab-content > div");
var cont = 0;

function filtrar(index) {
    span.style.display = 'none';

    if (index == 0) {
        tabs.forEach((tab) => {
            tab.classList.remove("filtrar-tabs");
            cont++;
        });
    } else {
        tabs.forEach((tab) => {
            if (tab.classList.contains("tab-item-" + index)) {
                tab.classList.remove("filtrar-tabs");
                cont++;
            } else {
                tab.classList.add("filtrar-tabs");
            }
        });
    }

    if (cont == 0) {
        span.style.display = 'block';
    }
    cont = 0;
}

function travelTo(latitud, longitud) {
    document.getElementById("open-left-panel").checked = false;
    document.getElementById("sidemenu-button").style.left = "0px";
    clicked = 0;
    map.flyTo([latitud, longitud], 16);
}