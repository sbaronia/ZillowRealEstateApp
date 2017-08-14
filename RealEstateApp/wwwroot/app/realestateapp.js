angular.module('RealEstateApp', [
    'RealEstateApp.controllers', 'RealEstateApp.services','ngRoute'
]).config(['$locationProvider', function ($locationProvider) {
    $locationProvider.hashPrefix('');
}]).config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
        when("/", { templateUrl: "/app/partials/realestateaddress.html", controller: "searchController" }).
        when("/address", { templateUrl: "/app/partials/realestateaddress.html", controller: "realestateAddressController" }).
        when("/estimate", { templateUrl: "/app/partials/realestateestimate.html", controller: "realestateEstimateController" }).
        when("/links", { templateUrl: "/app/partials/realestatelinks.html", controller: "realestateEstimateController" }).
        when("/neighborhoods", { templateUrl: "/app/partials/realestateneighborhoods.html", controller: "realestateEstimateController" }).
        otherwise({ redirectTo: '/' });
}]);