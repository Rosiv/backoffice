var backofficeApp = angular.module("backofficeApp", []);

backofficeApp.controller("backofficeCtrl", ['$scope', '$interval', '$http',
      function ($scope, $interval, $http) {
          $scope.data = "hello";
          var repeat = $interval(function () {
              $http({
                  method: 'GET',
                  url: '/jobs'
              }).then(function successCallback(response) {
                  if ($scope.data !== response.data)
                      $scope.data = response.data;
              }, function errorCallback(response) {
                  // called asynchronously if an error occurs
                  // or server returns response with an error status.
              });
          }, 500);

          $scope.retryJob = function (jobId) {
              url = '/jobs/' + jobId + '/retry';
              $http({
                  method: 'POST',
                  url: url
              }).then(function successCallback(response) {
              }, function errorCallback(response) {
                  job.Status = "Failed";
                  // called asynchronously if an error occurs
                  // or server returns response with an error status.
              });
          }
      }

]);