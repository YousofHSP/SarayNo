$('#addInvoiceModal').on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const projectId = button.data('project-id')
    const type = button.data('type')
    let modalBody = $(".invoice-modal-body")
    modalBody.find("[name=ProjectId]").val(projectId)
    modalBody.find("[name=Type]").val(type)

})
$("#addInvoice").click(function (){
    let modalBody = $(".invoice-modal-body")
    let data = {
        projectId: modalBody.find("[name=ProjectId]").val(),
        costGroupId: modalBody.find("[name=CostGroupId]").val(),
        creditorId: modalBody.find("[name=CreditorId]").val(),
        date : modalBody.find("[name=Date]").val(),
        description: modalBody.find("[name=Description]").val(),
        amount: modalBody.find("[name=Amount]").val(),
        type : modalBody.find("[name=Type]").val()
    }

    $.ajax({
        url: "/Admin/api/Invoice/Add",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (result) {
            Swal.fire({
                type: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            });
        }
    })

})
