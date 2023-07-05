
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

// Versión del mapa gratuita hasta 200k solicitudes al mes
/*L.tileLayer('https://api.mapbox.com/styles/v1/edgarpinedo/clfdrg9r1007t01p5sg0iffv0/tiles/256/{z}/{x}/{y}@2x?access_token=pk.eyJ1IjoiZWRnYXJwaW5lZG8iLCJhIjoiY2xmZHFtcTE2MGl2ejNycnJranlmd3I4ZyJ9.ocG7NBF7rlsU60Nq6WMbVQ', {
    attribution: `&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> | &copy; <a href="https://www.mapbox.com/about/maps/">Mapbox</a> contributors`,
}).addTo(map);*/

// Versión del mapa gratuita sin límite
L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

map.on('click', function (e) {
    document.getElementById("latitud").innerHTML = e.latlng.lat;
    document.getElementById("longitud").innerHTML = e.latlng.lng;
});

model.forEach(() => {
    L.marker([20.6696, -103.3405]).bindPopup("<b>hola</b><br><br>").addTo(map);
})

/*L.marker([20.6696, -103.3405]).bindPopup("<b>Categoría</b><br><br>bla bla blalalala bala balaa").addTo(map);*/

L.marker([20.6896, -103.3263]).bindPopup("<b>Categoría</b><br><br>bla bla blalalala bala balaa").addTo(map);