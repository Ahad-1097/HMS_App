$(document).ready(function () {
    $('#FilterDateBtn').click(function () {
        $('#FilterDate').toggle();
    });
    // Initialize datepicker for the start date input field
    flatpickr("#startDate", {
        dateFormat: "Y-m-d", // Set the date format
        onClose: function (selectedDates) {
            // Automatically close the calendar when a date is selected
            // this.close();
        }
    });
    flatpickr("#endDate", {
        dateFormat: "Y-m-d", // Set the date format
        onClose: function (selectedDates) {
            // Automatically close the calendar when a date is selected
            //this.close();
        }
    });

    // Initialize DataTable
    var table = $('#example5').DataTable();



    // Apply date range filter on button click
    $('#filterBtn').click(function () {
        var startDate = $('#startDate').val() != '' ? GetFormatedDate($('#startDate').val()) : '';
        var endDate = $('#endDate').val() != '' ? GetFormatedDate($('#endDate').val()) : '';

        // Custom filter function
        $.fn.dataTable.ext.search.push(
            function (settings, data, dataIndex) {
                var date = data[3] != '' ? compareDates(data[3]) : ''; // Assuming DOA is in the fourth column
                var startDateValid = startDate === '' || date >= startDate;
                var endDateValid = endDate === '' || date <= endDate;
                return startDateValid && endDateValid;
            }
        );



        // Apply the custom filter
        table.draw();

        // Remove the filter function from the stack to avoid duplicates
        $.fn.dataTable.ext.search.pop();
    });

    // Clear filters on button click
    $('#clearFilterBtn').click(function () {
        $('#startDate').val(''); // Clear start date input
        $('#endDate').val(''); // Clear end date input
        table.search('').draw(); // Clear DataTable search and redraw
    });


    function compareDates(date) {
        var parts = date.split('/');

        var day = parseInt(parts[1], 10);
        var month = parseInt(parts[0], 10) - 1; // Months are zero-based
        var year = parseInt(parts[2], 10);
        var dateObj = new Date(year, month, day);
        return dateObj;
    }

    function GetFormatedDate(date) {
        // Create a new Date object
        var currentDate = new Date(date);

        // Get day, month, and year components
        var day = currentDate.toLocaleString('en', { weekday: 'short' }); // Short weekday name
        var month = currentDate.toLocaleString('en', { month: 'short' }); // Short month name
        var date = currentDate.getDate(); // Day of the month
        var year = currentDate.getFullYear(); // Full year

        // Format the date string
        var dt = new Date(day + ' ' + month + ' ' + date + ' ' + year);
        return dt;
    }

});