angular.module('RealEstateApp.controllers', []).
    controller('searchController', function ($scope, searchAPIservice) {
        $scope.nameFilter = null;
        $scope.data = searchAPIservice.data;//{ "searchResult": null, "error": null };
        //$scope.data.error = { "error": 0, "status": "" };
        $scope.street = "";
        $scope.zipcode = "";
        $scope.city = "";
        $scope.state = "";
        $scope.detailHide = searchAPIservice.detailHide;
        $scope.$watch(function () { return searchAPIservice.detailHide }, function (newVal, oldVal) {
            if (typeof newVal !== 'undefined') {
                $scope.detailHide = searchAPIservice.detailHide;
            }
        });

        console.log("search controller started..");
        $scope.linkModelFunc = function (url) {
            console.log('link model function');
            $window.open(url);
        }
        $scope.validateSearchRequest = function () {
            searchAPIservice.validateSearchRequest($scope.street, $scope.city, $scope.state, $scope.zipcode);
            $scope.detailHide = searchAPIservice.detailHide;
        };
    }). 
    controller('realestateAddressController', function ($scope, searchAPIservice) {
        $scope.nameFilter = null;
        $scope.data = searchAPIservice.data;
        //$scope.detailHide = searchAPIservice.detailHide;

        console.log("realestateAddress controller started..");
    }).
    controller('realestateEstimateController', function ($scope, searchAPIservice) {
        $scope.data = searchAPIservice.data;//{ "searchResult": null, "error": null };
        $scope.linkModelFunc = function (url) {
            console.log('link model function');
            $window.open(url);
        }

        console.log("realestateEstimate controller started..");
    });