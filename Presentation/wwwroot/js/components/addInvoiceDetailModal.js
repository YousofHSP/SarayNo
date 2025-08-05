$('#addInvoiceDetailModal').on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const invoiceId = button.data('invoice-id')
    let modalBody = $(".invoice-detail-modal-body")
    modalBody.find("[name=InvoiceId]").val(invoiceId)

})
$("#addInvoiceDetail").click(function (){
    let modalBody = $(".invoice-detail-modal-body")
    let data = {
        invoiceId: modalBody.find("[name=InvoiceId]").val(),
        price: modalBody.find("[name=Price]").val(),
        description: modalBody.find("[name=Description]").val(),
        date : modalBody.find("[name=Date]").val(),
        type : modalBody.find("[name=Type]").val(),
    }

    $.ajax({
        url: "/Admin/api/Invoice/AddDetail",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (result) {
            Swal.fire({
                type: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            }).then(() => location.reload());
        }
    })

})
$('#addPayoffModal').on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const invoiceId = button.data('invoice-id')
    let modalBody = $(".payoff-modal-body")
    modalBody.find("[name=InvoiceId]").val(invoiceId)
})
$("#addPayoffModalBtn").click(function (){
    let modalBody = $(".payoff-modal-body")
    let data = {
        invoiceId: modalBody.find("[name=InvoiceId]").val(),
        price: modalBody.find("[name=Price]").val(),
        description: modalBody.find("[name=Description]").val(),
        date : modalBody.find("[name=Date]").val(),
        type : modalBody.find("[name=Type]").val(),
    }

    $.ajax({
        url: "/Admin/api/Invoice/AddPayoff",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (result) {
            Swal.fire({
                type: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            }).then(() => location.reload());;
        }
    })

})
