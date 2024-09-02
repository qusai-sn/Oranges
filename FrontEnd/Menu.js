function storeMealID(id) {
    localStorage.MealID = id;
  }
  let n = Number(localStorage.getItem("ResturantID"));
   let URL = `https://localhost:7147/api/Meals/${n}`;
   
  let container = document.getElementById("Products");
   
  async function GetMealsByResturantID() {
   
    let request = await fetch(URL);
    let response = await request.json();
  
    response.forEach((element) => {
      container.innerHTML += `
       <div class="card-body">
                  <div class="row">
                    <div class="col-md-12 col-lg-3 col-xl-3 mb-4 mb-lg-0">
                      <div
                        class="bg-image hover-zoom ripple rounded ripple-surface"
                      >
                        <img
                          src="${element.imageUrl}"
                          class="w-100"
                        />
                        <a href="#!">
                          <div class="hover-overlay">
                            <div
                              class="mask"
                              style="background-color: rgba(253, 253, 253, 0.15)"
                            ></div>
                          </div>
                        </a>
                      </div>
                    </div>
                    <div class="col-md-6 col-lg-6 col-xl-6">
                      <h5>${element.name}</h5>
                      
                      <div class="mt-1 mb-0 text-muted small">
                        
                      <p class="text-truncate mb-4 mb-md-0">
                      ${element.description}
                      </p>
                    </div>
                    <div
                      class="col-md-6 col-lg-3 col-xl-3 border-sm-start-none border-start"
                    >
                      <div class="d-flex flex-row align-items-center mb-1">
                        <span >${element.price}دينار</span>
                      </div>
                      <div class="d-flex flex-column mt-4">
                        <button
                          data-mdb-button-init
                          data-mdb-ripple-init
                          class="btn btn-primary btn-sm"
                          type="button"
                        >
                          Details
                        </button>
                        <button
                          data-mdb-button-init
                          data-mdb-ripple-init
                          class="btn btn-outline-primary btn-sm mt-2"
                          type="button"
                        >
                          Add to wishlist
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
      
      
      
      `;
    });
  }
  GetMealsByResturantID();