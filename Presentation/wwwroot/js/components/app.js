$(".changeType").click(function () {
    let id = $(this).data("id");
    let type = $(this).data("type");
    let data = {type: type};
    $.ajax({
        url: `/Admin/api/Invoice/${id}/ChangeType?type=${type}`,
        method: "get",
        success: result => {
            Swal.fire({
                type: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            }).then(res => location.reload());
        }
    })
})
$(".changeStatus").click(function () {
    let id = $(this).data("id");
    let status = $(this).data("status");
    let modalBody = $(".change-status-modal-body");
    modalBody.find("[name=Id]").val(id);
    modalBody.find("[name=Status]").val(status);
    $("#changeStatusModal").modal("show");
})
$("#changeStatusBtn").click(function () {

    let modalBody = $(".change-status-modal-body");
    let id = modalBody.find("[name=Id]").val();
    let status = Number(modalBody.find("[name=Status]").val());
    let description = modalBody.find("[name=Description]").val();
    let data = {status, id, description};
    $.ajax({
        url: `/Admin/api/Invoice/ChangeStatus`,
        contentType: 'application/json',
        data: JSON.stringify(data),
        method: "post",
        success: () => {
            Swal.fire({
                type: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            }).then(() => location.reload());
        }
    })
})
$("#addPayoff").click(function () {
    let form = $("#addPayoffForm");
    let invoiceId = form.find("[name=InvoiceId]").val();
    let type = form.find("[name=type]").val();
    let description = form.find("[name=description]").val();
    let date = form.find("[name=date]").val();
    let price = form.find("[name=price]").val();
    let data = {invoiceId, type, description, date, price};
    $.ajax({
        url: `/Admin/api/Invoice/AddPayoff`,
        contentType: 'application/json',
        data: JSON.stringify(data),
        method: "post",
        success: (res) => {
            let payoffsEl = `
            <tr>
                <td>${res.typeDisplay}</td>
                <td>${res.description}</td>
                <td>${res.date}</td>
                <td colspan="2">${res.price}</td>
            </tr>
        `
            $('#payoffs-table').append(payoffsEl)
            Swal.fire({
                type: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            })

        }
    })
})
