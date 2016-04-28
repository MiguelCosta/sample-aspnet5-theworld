(function () {
    'use strict';

    angular
        .module('app-trips')
        .controller('tripsController', tripsController);

    //tripsController.$inject = ['$location'];

    function tripsController($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.name = 'Miguel Costa';
        vm.trips = [
            {
                name: "US Trip",
                created: new Date()
            },
            {
                name: "World Trip",
                created: new Date()
            }

        ];

        vm.newTrip = {};

        vm.addTrip = function () {
            alert(vm.newTrip.name);
        };

        //activate();

        //function activate() { }
    }
})();
