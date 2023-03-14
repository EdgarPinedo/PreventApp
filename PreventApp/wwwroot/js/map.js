
var mapOptions = {
    center: [20.6696, -103.3405],
    zoom: 13,
    minZoom:12,
    maxZoom: 15,
    zoomControl: false
}

var map = L.map('map', mapOptions);
L.control.zoom({ position: "bottomright" }).addTo(map);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);


L.marker([20.6696, -103.3405]).bindPopup("<b>Categoría</b><br><br>bla bla blalalala bala balaa").addTo(map);

L.marker([20.6896, -103.3263]).bindPopup("<b>Categoría</b><br><br>bla bla blalalala bala balaa").addTo(map);