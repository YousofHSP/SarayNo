$('#itemModal').on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const id = button.data('bs-id')
    const type = button.data('bs-type')
    const modal = $(this)
    let el = ``;
    let typeNum = type == "Estimate" ? 0 : 1
    items.filter(i => i.ProjectDetailId == id && i.Type == typeNum).forEach(item => {
        el += `<tr>
            <td>${item.Title}</td>
            <td>${item.Area}</td>
            <td>${item.UnitPrice}</td>
            <td>${item.TotalPrice}</td>
            <td>${item.Description}</td>
        </tr>`
    })
    modal.find('table tbody').html(el)
    
    modal.find('[name=ProjectDetailId]').val(id)
    modal.find('[name=Type]').val(type)
})
$("#editModal").on("show.bs.modal", function(event) {
    const button = $(event.relatedTarget)
    const id = button.data('id')
    const type = button.data('type')
    const percent= button.data('percent')
    const date = button.data('date')
    const description= button.data('description')
    const title = button.data('title')
    const modal = $(this)
    modal.find('[name=Id]').val(id)
    modal.find('[name=Percent]').val(percent)
    modal.find('[name=Title]').val(title)
    modal.find('[name=Date]').val(date)
    modal.find('[name=Description]').val(description)
    modal.find('[name=Type]').val(type)
})
$('[name=UnitPrice], [name=Area]').on('keyup', function(){
    
    let unit = $('[name=UnitPrice]').val() ?? 0;
    let area = $('[name=Area]').val() ?? 0;
    $('[name=TotalPrice]').val(unit * area);
    
})