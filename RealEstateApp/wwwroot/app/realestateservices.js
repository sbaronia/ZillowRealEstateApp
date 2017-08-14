angular.module('RealEstateApp.services', []).
    factory('searchAPIservice', function ($http) {

        var searchAPI = {
            data: { searchResult: null, error: { error: 0, status: "" } }, detailHide: 1
        };
    

        searchAPI.getRealEstateDetails = function (street, city, state, zipcode) {
            var request = null;
            if (city.length > 0 && state.length > 0) {
                // Your Code
                request = searchAPI.searchByCityState(street, city, state);
            } else {

                request = searchAPI.searchByZipcode(street, zipcode);
            }

            request.then(function (response) {
                //Dig into the responde to get the relevant data
                //console.log(response);
                switch (response.data.error) {
                    case 0:
                        searchAPI.data.error.status = "Data available cool...";
                        searchAPI.data.error.error = 200;
                        searchAPI.data.searchResult = response.data.results[0];
                        //$location.path("/details");
                        console.log(searchAPI.data.searchResult);
                        searchAPI.detailHide = 0;
                        break;
                    case 1: // SERVER_ERROR
                        searchAPI.data.error.status = "System error";
                        searchAPI.data.error.error = 500;
                        break;
                    case 2: //MISSING_ADDRESS,
                        searchAPI.data.error.status = "Street missing";
                        searchAPI.data.error.error = 422;
                        break;
                    case 3: //MISSING_CITY_STATE,
                        searchAPI.data.error.status = "City missing";
                        searchAPI.data.error.error = 422;
                        break;
                    case 4: //MISSING_ZIP,
                        searchAPI.data.error.status = "Zipcode missing";
                        searchAPI.data.error.error = 422;
                        break;
                    case 5: //INVALID_ADDRESS,
                        searchAPI.data.error.status = "Invalid address";
                        searchAPI.data.error.error = 422;
                        break;
                    case 6: //SERVICE_NO_AVALIABLE,
                        searchAPI.data.error.status = "Service not available";
                        searchAPI.data.error.error = 500;
                        break;
                    case 7: //NO_RESULTS,
                        searchAPI.data.error.status = "No records found";
                        searchAPI.data.error.error = 204;
                        break;
                    case 8: //NO_SERVICE,
                        searchAPI.data.error.status = "No Service in the region";
                        searchAPI.data.error.error = 500;
                        break;
                    case 9: //LIMIT_REACHED,
                        searchAPI.data.error.status = "Request Limit warning";
                        searchAPI.data.error.error = 500;
                        break;
                    default:
                        searchAPI.data.error.status = "System error";
                        searchAPI.data.error.error = 500;
                }
                console.log("Response {" + searchAPI.data.error.error + "," + searchAPI.data.error.status + "}");
            }, function (error, status) {
                searchAPI.data.error.error = status;
                searchAPI.data.error.status = error;

                console.log(searchAPI.data.error.status);
            });
        }

            searchAPI.searchByCityState = function (street, city, state) {
                    return $http.get('/api/realestate/address/' + street + '/city/' + city + '/state/' + state);
            }

            searchAPI.searchByZipcode = function (street, zipcode) {
                return $http.get('/api/realestate/address/' + street + '/zipcode/' + zipcode);
            }

            searchAPI.validateRequest = function (street, city, state, zipcode) {
                var processContinue = false;
                var error = "";
                console.log("Validate request made.")
                console.log("street:" + street);
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

            searchAPI.validateSearchRequest = function (street, city, state, zipcode) {
                var result = searchAPI.validateRequest(street, city, state, zipcode);
                searchAPI.detailHide = 1;
                if (result.result) {
                    console.log("Sending Request to server...");
                    searchAPI.data.error.status = "Sending Request to server...";
                    searchAPI.data.error.error = 0;

                    searchAPI.getRealEstateDetails(street, city, state, zipcode);
                } else {
                    searchAPI.data.error.status = result.error;
                }
            }
        
        return searchAPI;
    });