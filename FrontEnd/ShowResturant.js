let URL = " https://localhost:7147/api/Meals/Resturants";

function storeResturantID(id) {
  localStorage.ResturantID = id;
}

async function GetAllResturants() {
  let response = await fetch(URL);
  let data = await response.json();
  console.log(data);

  let container = document.getElementById("Resturant");
  data.forEach((element) => {
    container.innerHTML +=` 
        <div class="col-md-4 mb-4">
            <div class="card card-custom">
                <img src="${element.imageUrl}" class="card-img-top" alt="Image">
                <div class="card-body">
                    <h5 class="card-title">${element.name}</h5>
                    <p class="card-text">${element.description}</p>
                    <a href="Menu.html" class="btn btn-primary"  onclick="storeResturantID(${element.id})">
تصفّح القائمة
                    </a>

                </div>
            </div>
        </div>`;
  });
}

GetAllResturants();