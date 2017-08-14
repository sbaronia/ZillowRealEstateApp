angular.module('RealEstateApp.controllers', []).
    controller('searchController', function ($scope, searchAPIservice) {
        $scope.nameFilter = null;
        $scope.data = { "searchResult": null, "error": null };
        $scope.data.error = { "error": 0, "status": "" };
        $scope.street = "";
        $scope.zipcode = "";
        $scope.city = "";
        $scope.state = "";
        $scope.detailHide = 1;

        console.log("search controller started..");
        $scope.linkModelFunc = function (url) {
            console.log('link model function');
            $window.open(url);
        }
        $scope.validateSearchRequest = function () {
            var result = searchAPIservice.validateRequest($scope.street, $scope.city, $scope.state, $scope.zipcode);
            $scope.detailHide = 1;
            if (result.result) {
                console.log("Sending Request to server...");
                $scope.data.error.status = "Sending Request to server...";
                $scope.data.error.error = 0;
                
               searchAPIservice.getRealEstateDetails($scope.street, $scope.city, $scope.state, $scope.zipcode).then(function (response) {
                    //Dig into the responde to get the relevant data
                    //console.log(response);
                   switch (response.data.error) {
                       case 0:
                           $scope.data.error.status = "Data available cool...";
                           $scope.data.error.error = 200;
                           $scope.data.searchResult = response.data.results[0];
                           //$location.path("/details");
                           console.log($scope.data.searchResult);
                           $scope.detailHide = 0;
                           break;
                       case 1: // SERVER_ERROR
                           $scope.data.error.status = "System error";
                           $scope.data.error.error = 500;
                           break;
                       case 2: //MISSING_ADDRESS,
                           $scope.data.error.status = "Street missing";
                           $scope.data.error.error = 422;
                           break;
                       case 3: //MISSING_CITY_STATE,
                           $scope.data.error.status = "City missing";
                           $scope.data.error.error = 422;
                           break;
                       case 4: //MISSING_ZIP,
                           $scope.data.error.status = "Zipcode missing";
                           $scope.data.error.error = 422;
                           break;
                       case 5: //INVALID_ADDRESS,
                           $scope.data.error.status = "Invalid address";
                           $scope.data.error.error = 422;
                           break;
                       case 6: //SERVICE_NO_AVALIABLE,
                           $scope.data.error.status = "Service not available";
                           $scope.data.error.error = 500;
                           break;
                       case 7: //NO_RESULTS,
                           $scope.data.error.status = "No records found";
                           $scope.data.error.error = 204;
                           break;
                       case 8: //NO_SERVICE,
                           $scope.data.error.status = "No Service in the region";
                           $scope.data.error.error = 500;
                           break;
                       case 9: //LIMIT_REACHED,
                           $scope.data.error.status = "Request Limit warning";
                           $scope.data.error.error = 500;
                           break;
                        default:
                           $scope.data.error.status = "System error";
                           $scope.data.error.error = 500;
                   }                    
                   console.log("Response {" + $scope.data.error.error+","+$scope.data.error.status+"}");
                },function (error, status) {
                    $scope.data.error.error = status;
                    $scope.data.error.status = error;

                    console.log($scope.data.error.status);
                });
            } else {
                $scope.data.error.status = result.error;
            }
        };
    }).  /* Driver controller */
    controller('searchDetailsController', function ($scope, searchAPIservice) {

    });