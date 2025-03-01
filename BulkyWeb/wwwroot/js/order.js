$(document).ready(function ()
{
    var url = window.location.search;
    if (url.includes("inprocess"))
    {
        loadDataTable("inprocess");
    }
    else
    {
        if (url.includes("pending"))
        {
            loadDataTable("pending");
        }

        else
        {
            if (url.includes("approved"))
            {
                loadDataTable("approved");
            }

            else
            {
                if (url.includes("completed"))
                {
                    loadDataTable("completed");
                }

                else
                {
                    if (url.includes("cancelled"))
                    {
                        loadDataTable("cancelled");
                    }

                    else
                    {
                        loadDataTable("all");
                    }
                }
            }

        }
    }   
    
});

function loadDataTable(status) {
    $('#orderTable').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        "columns": [
            { data: 'id',"width":"5%" },
            { data: 'name', "width": "10%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'applicationUser.email', "width": "10%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'totalOrder', "width": "10%" },
            {
                data: 'id', "render": function (data) {
                    return `<div class="w-75 btn btn-group " role="group">  
                   <a href="/admin/order/Details?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> </a>
                    </div >`


                },

                "width": "20%"
            }
        ]
    });

};


    

                

