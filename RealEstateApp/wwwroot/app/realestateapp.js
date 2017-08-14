angular.module('RealEstateApp', [
    'RealEstateApp.controllers', 'RealEstateApp.services','ngRoute'
]).config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
        when("/realestate", { templateUrl: "/app/partials/searchdetails.html", controller: "searchController" }).
        //when("/details", { templateUrl: "/app/partials/searchdetails.html", controller: "searchDetailsController" }).
        otherwise({ redirectTo: '/realestate' });
}]);