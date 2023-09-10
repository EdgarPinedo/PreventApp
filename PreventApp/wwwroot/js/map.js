
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
    3: "Semáforo defectuoso"
};

var icons = {
    1: '<i class="bi bi-car-front-fill"></i>',
    2: '<i class="bi bi-fire"></i>',
    3: '<i class="bi bi-stoplights"></i>'
};

var markers = {
    1: 'images/trafico-icon.png',
    2: 'images/fire-icon.png',
    3: 'images/semaforo-icon.png'
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