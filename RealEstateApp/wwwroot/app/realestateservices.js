angular.module('RealEstateApp.services', []).
    factory('searchAPIservice', function ($http) {

        var searchAPI = {};

        searchAPI.getRealEstateDetails = function (street, city, state, zipcode) {
            if (city.length > 0 && state.length > 0) {
                // Your Code
                return $http.get('/api/realestate/address/' + street + '/city/' + city + '/state/' + state);
            } 

            return $http.get('/api/realestate/address/'+street+'/zipcode/'+zipcode);
        }

        searchAPI.validateRequest = function (street, city, state, zipcode) {
            var processContinue = false;
            var error = "";
            console.log("Validate request made.")
            console.log("street:"+street);
            console.log("city:" + city);
            console.log("state:" + state);
            console.log("zipcode:" + zipcode);

            street = street.trim();
            city = city.trim();
            state = state.trim();
            zipcode = zipcode.trim();
            if (street.length > 0) {
                if ((city.length > 0 && state.length > 0) || (zipcode.length > 0)) {
                    processContinue = true;
                } else if (city.length == 0 && state.length > 0) {
                    error = "City is missing";
                } else if (state.length == 0 && city.length > 0) {
                    error = "State is missing";
                } else {
                    error = "City/State or Zipcode is missing";
                }
            } else {
                error = "Street name is missing";
            }

            return { "result": processContinue, "error": error };
        }

        return searchAPI;
    });