// tripEditorController.js
(function () {
    "use strict";

    angular.module("app-trips")
        .controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $http) {
        var vm = this;

        vm.tripName = $routeParams.tripName;
        vm.stops = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.newStop = {};

        var url = "/api/trips/" + vm.tripName + "/stops";

        $http.get(url)
            .then(function (response) {
                console.log(JSON.stringify(response));
                angular.copy(response.data, vm.stops);
                _showMap(vm.stops);
            }, function (error) {
                vm.errorMessage = "Failed load stops: " + error;
            }).finally(function () {
                vm.isBusy = false;
            });

        vm.addStop = function () {
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post(url, vm.newStop)
              .then(function (response) {
                  // success
                  vm.stops.push(response.data);
                  vm.newStop = {};
                  _showMap(vm.stops);
              }, function (err) {
                  // failure
                  vm.errorMessage = "Failed to save new stop: " + err;
              })
              .finally(function () {
                  vm.isBusy = false;
              });
        }
    }

    function _showMap(stops) {
        if (stops && stops.length > 0) {
            var mapStops = _.map(stops, function (item) {
                var ms = {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
                return ms;
            });

            console.log(JSON.stringify(mapStops));
            // show map
            travelMap.createMap({
                stops: mapStops,
                selector: "#map", // local inde the page
                initialZoom: 5
            });
        }
    }
})();
