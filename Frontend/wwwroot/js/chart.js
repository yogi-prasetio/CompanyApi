$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: "https://localhost:7034/api/Department/EmployeStatusPerDepartment",
        dataType: 'json',
        dataSrc: 'data',
        success: function (response) {
            var departmentName = response.data.map(function (item) {
                return item.departmentName;
            });

            var activeEmployee = response.data.map(function (item) {
                return item.activeEmployeeCount;
            });

            var resignEmployee = response.data.map(function (item) {
                return item.resignEmployeeCount;
            });

            var areaChartData = {
                labels: departmentName,
                datasets: [
                    {
                        label: 'Active',
                        backgroundColor: 'rgba(60,141,188,0.9)',
                        borderColor: 'rgba(60,141,188,0.8)',
                        pointRadius: false,
                        pointColor: '#3b8bba',
                        pointStrokeColor: 'rgba(60,141,188,1)',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(60,141,188,1)',
                        data: activeEmployee
                    },
                    {
                        label: 'Resign',
                        backgroundColor: 'rgba(184, 20, 20, 1)',
                        borderColor: 'rgba(184, 20, 20, 1)',
                        pointRadius: false,
                        pointColor: 'rgba(184, 20, 20, 1)',
                        pointStrokeColor: '#c1c7d1',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(184, 20, 20, 1)',
                        data: resignEmployee
                    },
                ],
            }

            var barChartCanvas = $('#barChart').get(0).getContext('2d')

            var barChartData = $.extend(true, {}, areaChartData)
            var temp0 = areaChartData.datasets[0]
            var temp1 = areaChartData.datasets[1]
            barChartData.datasets[0] = temp0
            barChartData.datasets[1] = temp1

            var barChartOptions = {
                responsive: true,
                maintainAspectRatio: false,
                datasetFill: false,
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                        }
                    }]
                }
            };

            new Chart(barChartCanvas, {
                type: 'bar',
                data: barChartData,
                options: barChartOptions
            })            
        }
    });

    $.ajax({
        type: 'GET',
        url: "https://localhost:7034/api/Department/EmployePerDepartment",
        dataType: 'json',
        dataSrc: 'data',
        success: function (response) {
            var departmentName = response.data.map(function (item) {
                return item.departmentName;
            });

            var employeesCount = response.data.map(function (item) {
                return item.employeesCount;
            });

            //-------------
            //- DONUT CHART -
            //-------------
            // Get context with jQuery - using jQuery's .get() method.
            var donutChartCanvas = $('#donutChart').get(0).getContext('2d')
            var donutData = {
                labels: departmentName,
                datasets: [
                    {
                        data: employeesCount,
                        backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc'],
                    }
                ]
            }
            var donutOptions = {
                maintainAspectRatio: false,
                responsive: true,
            }
            //Create pie or douhnut chart
            // You can switch between pie and douhnut using the method below.
            /*PIE CHART*/
            var donutData = {
                labels: departmentName,
                datasets: [
                    {
                        data: employeesCount,
                        backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc'],
                    }
                ]
            }

            var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
            var pieData = donutData;
            var pieOptions = {
                maintainAspectRatio: false,
                responsive: true,
            }
            //Create pie or douhnut chart
            // You can switch between pie and douhnut using the method below.
            new Chart(pieChartCanvas, {
                type: 'pie',
                data: pieData,
                options: pieOptions
            })
        }
    });

    $.ajax({
        type: 'GET',
        url: "https://localhost:7034/api/Employee/ActiveEmployeeCount",
        dataType: 'json',
        dataSrc: 'data',
        success: function (response) {
            var departmentName = response.data.map(function (item) {
                return item.departmentName;
            });

            var employeesCount = response.data.map(function (item) {
                return item.employeesCount;
            });

            //-------------
            //- DONUT CHART -
            //-------------
            // Get context with jQuery - using jQuery's .get() method.
            var donutChartCanvas = $('#donutChart').get(0).getContext('2d')
            var donutData = {
                labels: departmentName,
                datasets: [
                    {
                        data: employeesCount,
                        backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc'],
                    }
                ]
            }
            var donutOptions = {
                maintainAspectRatio: false,
                responsive: true,
            }
            //Create pie or douhnut chart
            // You can switch between pie and douhnut using the method below.
            new Chart(donutChartCanvas, {
                type: 'doughnut',
                data: donutData,
                options: donutOptions
            })
        }
    });
})