let itemIndex = 1;

function calculateTotal(index) {
    const quantity = parseInt(document.getElementsByName(`orderItems[${index}].Quantity`)[0].value) || 0;
    const itemId = document.getElementsByName(`orderItems[${index}].ItemId`)[0].value;

    if (itemId) {
        fetch(`/Items/Details/${itemId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(item => {
                const totalAmount = item.Price * quantity;
                document.getElementsByName(`orderItems[${index}].TotalAmount`)[0].value = totalAmount.toFixed(2);
            })
            .catch(error => console.error('Error fetching item details:', error));
    } else {
        document.getElementsByName(`orderItems[${index}].TotalAmount`)[0].value = "";
    }
}

function addItem() {
    const container = document.getElementById("itemContainer");
    const newItem = document.createElement("div");
    newItem.innerHTML = `
        <select name="orderItems[${itemIndex}].ItemId" onchange="calculateTotal(${itemIndex})">
            <option value="">Select Item</option>
            @foreach (var item in ViewBag.Items)
            {
                <option value="@item.Id">@item.Name</option>
            }
        </select>
        <input type="number" name="orderItems[${itemIndex}].Quantity" placeholder="Quantity" oninput="calculateTotal(${itemIndex})" />
        <input type="text" name="orderItems[${itemIndex}].TotalAmount" placeholder="Total Amount" readonly />
    `;
    container.appendChild(newItem);
    itemIndex++;
}
