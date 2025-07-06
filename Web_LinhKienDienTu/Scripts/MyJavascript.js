

document.addEventListener('DOMContentLoaded', function () {
 
    let slideIndex = 0;
    const slides = document.querySelectorAll('.slide');
    const dots = document.querySelectorAll('.dot');
 
    if (slides.length > 0 && dots.length > 0) {
        function showSlide(index) {
            slideIndex = index;
            slides.forEach((slide, i) => {
                slide.classList.remove('active-slide');
                if (dots[i]) {
                    dots[i].classList.remove('active');
                }
            });
            slides[slideIndex].classList.add('active-slide');
            if (dots[slideIndex]) {
                dots[slideIndex].classList.add('active');
            }
        }

        function autoSlide() {
            slideIndex = (slideIndex + 1) % slides.length;
            showSlide(slideIndex);
        }
 
        showSlide(slideIndex);
        setInterval(autoSlide, 3000);
    } else {
        console.info("Không tìm thấy các phần tử .slide hoặc .dot. Chức năng slideshow sẽ không hoạt động trên trang này.");
    }

    const btn = document.getElementById('DangNhapGioHang');
    if (btn) {
        btn.addEventListener('click', function () {
            if (isLoggedIn === "false") {
                Swal.fire({
                    icon: 'warning',
                    title: 'Bạn chưa đăng nhập',
                    text: 'Vui lòng đăng nhập để xem giỏ hàng!',
                    confirmButtonText: 'Đăng nhập'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = '/Home/DangNhap';
                    }
                });
            } else {
                window.location.href = '/Home/ThongTinGioHang';  
            }
        });
    }

    function addToCart(sp) {
        const cart = JSON.parse(localStorage.getItem('cart')) || [];
        cart.push(sp);
        localStorage.setItem('cart', JSON.stringify(cart));
        alert("Đã thêm vào giỏ hàng!");
    }
  
    const buttonCreative = document.querySelector('.button-creative');
    if (buttonCreative) {
        buttonCreative.addEventListener('mousemove', (e) => {
            const rect = buttonCreative.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;
            buttonCreative.style.setProperty('--x', `${x}px`);
            buttonCreative.style.setProperty('--y', `${y}px`);
        });
    } else {
        console.info("Không tìm thấy button-creative trên trang này.");
    }




    function displayItemsToWrapper(items, wrapperId) {
        const wrapper = document.getElementById(wrapperId);
        if (wrapper && items) { 
            wrapper.innerHTML = "";
            items.forEach(item => wrapper.appendChild(item));
        } else {
            console.warn(`Không tìm thấy wrapper với ID "${wrapperId}" hoặc items rỗng.`);
        }
    }


    //===========doanh thu=========
    // ========== Kiểm tra và vẽ biểu đồ nếu có phần tử ==========

    const chartCanvas = document.getElementById('myPieChart');
    if (chartCanvas) {
        const ctx = chartCanvas.getContext('2d');
        const myPieChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: [
                    'Doanh thu theo ngày/tháng/năm',
                    'Doanh thu theo loại sản phẩm/dịch vụ',
                    'Doanh thu theo kênh bán hàng',
                    'Lợi nhuận ròng'
                ],
                datasets: [{
                    data: [25, 30, 35, 10],
                    backgroundColor: ['orange', 'purple', 'green', 'gold'],
                    borderColor: 'white',
                    borderWidth: 2
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'top'
                    }
                }
            }
        });
    }

    // ========== Check All ==========

    const checkAll = document.getElementById("checkAll");
    if (checkAll) {
        checkAll.addEventListener("click", function () {
            let checkboxes = document.querySelectorAll("input[name='selectedIds']");
            for (let cb of checkboxes) {
                cb.checked = this.checked;
            }
        });
    }

    //==========LLoc san pham
    const searchInput = document.getElementById("searchInput");
    const filterLoai = document.getElementById("filterLoai");
    const rows = document.querySelectorAll("#productTable tbody tr");

    if (searchInput) {
        function filterTable() {
            const keyword = searchInput.value.toLowerCase();
            const loai = filterLoai.value;

            rows.forEach(row => {
                const tenSP = row.cells[1].textContent.toLowerCase();
                const loaiSP = row.cells[3].textContent;

                const matchTen = tenSP.includes(keyword);
                const matchLoai = (loai === "all" || loaiSP === loai);

                if (matchTen && matchLoai) {
                    row.style.display = "";
                } else {
                    row.style.display = "none";
                }
            });
        }
        searchInput.addEventListener("input", filterTable);
        filterLoai.addEventListener("change", filterTable);
    }

    
    // ===== 1. Tạo danh sách sản phẩm (dạng card) phân trang (sử dụng sanPhamData) =====
    // Chỉ chạy nếu sanPhamData được định nghĩa và có dữ liệu
    if (typeof sanPhamData !== 'undefined' && sanPhamData && sanPhamData.length > 0) {
        const itemsPerPage = 8;
        let currentPage = 1;

        const dataContainer = document.getElementById("data-container");
        const paginationContainer = document.getElementById("pagination");

        if (dataContainer && paginationContainer) {
            
            const allProducts = [...sanPhamData];

             
            const featuredProducts = [...allProducts]
                .sort((a, b) => {
                    
                    const priceA = parseFloat(a.GiaHT);
                    const priceB = parseFloat(b.GiaHT);
                    return priceB - priceA;  
                })
                .slice(0, 8);  
            
            const discountedProducts = allProducts.slice(8, 16);  

             
            const combinedProducts = [...featuredProducts, ...discountedProducts];


            const items = combinedProducts.map((sp) => {  
                const col = document.createElement('div');
                col.className = 'col-lg-3 col-6 mb-4 ';

                const card = document.createElement('div');
                card.className = 'card h-100 d-flex flex-column border  border-2 hover-border-thick text-success ';
                card.addEventListener('click', function () {
                    window.location.href = `/Home/ChiTietSanPham/${sp.Id}`;
                });

                const img = document.createElement('img');
                img.src = sp.Img;
                img.className = 'card-img-top';
                img.alt = sp.TenSP || 'Product image';

                const body = document.createElement('div');
                body.className = 'card-body d-flex flex-column';

                const title = document.createElement('h6');
                title.className = 'card-title';
                title.innerText = sp.TenSP;

                const price = document.createElement('p');
                price.className = 'card-text text-danger mb-1';
                price.innerText = sp.GiaHT;

                const original = document.createElement('p');
                original.className = 'card-text text-muted';
                original.innerHTML = `<s>${sp.GiaGoc}</s>`;

                const chillDiv = document.createElement('div');
                chillDiv.className = 'd-flex gap-2 mt-auto';

                const icon = document.createElement('i');
                icon.className = 'fas fa-shopping-cart align-self-center text-danger';

                const button = document.createElement('button');
                button.className = 'btn btn-danger flex-grow-1';
                button.type = 'submit';
                button.innerText = ' Thêm vào giỏ';
                button.setAttribute('data-masp', sp.Id);

                button.addEventListener('click', function () {
                    if (isLoggedIn === "false") {
                        Swal.fire({
                            icon: 'warning',
                            title: 'Bạn chưa đăng nhập',
                            text: 'Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng!',
                            confirmButtonText: 'Đăng nhập'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = '/Home/DangNhap';
                            }
                        });
                    }
                    else {
                        addToCart(sp);
                        //alert("SP gửi đi:" + sp.Id + ' ' + sp.TenSP + ' ' + sp.GiaGoc + ' ' + sp.GiaHT + ' ' + sp.Img + ' ' + sp.soLuong);
                        sendToCardSever(sp);
                    }
                })
                

                chillDiv.appendChild(icon);
                chillDiv.appendChild(button);

                body.appendChild(title);
                body.appendChild(price);
                body.appendChild(original);
                body.appendChild(chillDiv);

                card.appendChild(img);
                card.appendChild(body);
                col.appendChild(card);

                return col;
            });


            function displayPaginatedItems(items, wrapper, rowsPerPage, page) {
                wrapper.innerHTML = "";
                const start = (page - 1) * rowsPerPage;
                const end = start + rowsPerPage;
                const paginatedItems = items.slice(start, end);
                paginatedItems.forEach(item => wrapper.appendChild(item));
            }
            let s1 = "Sản phẩm nổi bật", s2 = "Sản phẩm khuyến mãi"; // Sửa chính tả
            function setupPagination(items, wrapper, rowsPerPage) {
                wrapper.innerHTML = "";
                const pageCount = Math.ceil(items.length / rowsPerPage);
                const ul = document.createElement("ul");
                ul.className = "pagination justify-content-center";


                for (let i = 1; i <= pageCount; i++) {
                    const li = document.createElement("li");
                    li.className = "page-item" + (i === currentPage ? " active" : "");

                    const btn = document.createElement("button");
                    btn.className = "page-link";
                    if (i === 1) { 
                        btn.innerText = s1; 
                    } else if (i === 2) {  
                        btn.innerText = s2;
                    } else {
                        btn.innerText = `Trang ${i}`; 
                    }


                    btn.addEventListener("click", () => {
                        currentPage = i;
                        displayPaginatedItems(items, dataContainer, rowsPerPage, currentPage);
                        setupPagination(items, wrapper, rowsPerPage);
                    });

                    li.appendChild(btn);
                    ul.appendChild(li);
                }
                wrapper.appendChild(ul);
            }

            displayPaginatedItems(items, dataContainer, itemsPerPage, currentPage);
            setupPagination(items, paginationContainer, itemsPerPage);
        } else {
            console.info("Trang này không có phần Sản phẩm (dạng card) phân trang.");
        }
    } else {
        console.info("Biến sanPhamData không được định nghĩa hoặc rỗng trên trang này.");
    }
     


    // ===== 2. Tạo danh sách 7 sản phẩm nổi bật (sử dụng sanPhamData) =====
    // Chỉ chạy nếu sanPhamData được định nghĩa và có dữ liệu
    if (typeof sanPhamData !== 'undefined' && sanPhamData && sanPhamData.length > 0) {
        const items1 = sanPhamData.slice(0, 10).map((sp) => {
            const container = document.createElement('div');
            container.className = "row align-items-center mb-3  hover-border-thick text-success";

            const imgCol = document.createElement('div');
            imgCol.className = "col-4";
            const img = document.createElement('img');
            img.src = sp.Img;
            img.className = "img-fluid";
            img.alt = sp.TenSP || 'Product image';
            imgCol.appendChild(img);

            const infoCol = document.createElement('div');
            infoCol.className = "col-8";

            const title = document.createElement('h6');
            title.className = 'mb-1';
            title.innerText = sp.TenSP;

            const price = document.createElement('p');
            price.className = 'text-danger mb-1';
            price.innerText = sp.GiaHT;

            const original = document.createElement('p');
            original.className = 'text-muted mb-0';
            original.innerHTML = `<s>${sp.GiaGoc}</s>`;

            infoCol.appendChild(title);
            infoCol.appendChild(price);
            infoCol.appendChild(original);

            container.appendChild(imgCol);
            container.appendChild(infoCol);
            container.addEventListener('click', function () {
                window.location.href = `/Home/ChiTietSanPham/${sp.Id}`;
            });

            return container;
        });

        displayItemsToWrapper(items1, "bestseller"); 
    } else {
        console.info("Biến sanPhamData không được định nghĩa hoặc rỗng cho phần sản phẩm nổi bật.");
    }


    // ===== 3. Hiển thị danh sách 12 sản phẩm khác (KHÔNG có nút thêm vào giỏ) (sử dụng sanPhamData) =====
    // Chỉ chạy nếu sanPhamData được định nghĩa và có dữ liệu
    if (typeof sanPhamData !== 'undefined' && sanPhamData && sanPhamData.length > 0) {
        const items2 = sanPhamData.slice(0, 12).map((sp) => {
            const col = document.createElement('div');
            col.className = "col mb-4";

            const card = document.createElement('div');
            card.className = 'card h-100 d-flex flex-column border  border-2 hover-border-thick text-success';

            const img = document.createElement('img');
            img.src = sp.Img;
            img.className = 'card-img-top';
            img.alt = sp.TenSP || 'Product image';

            const body = document.createElement('div');
            body.className = 'card-body d-flex flex-column';

            const title = document.createElement('h6');
            title.className = 'card-title';
            title.innerText = sp.TenSP;

            const price = document.createElement('p');
            price.className = 'card-text text-danger mb-1';
            price.innerText = sp.GiaHT;

            const original = document.createElement('p');
            original.className = 'card-text text-muted';
            original.innerHTML = `<s>${sp.GiaGoc}</s>`;

            body.appendChild(title);
            body.appendChild(price);
            body.appendChild(original);

            card.appendChild(img);
            card.appendChild(body);
            col.appendChild(card);

            col.addEventListener('click', function () {
                window.location.href = `/Home/ChiTietSanPham/${sp.Id}`;
            });


            return col;
        });

        displayItemsToWrapper(items2, "data-container1");
    } else {
        console.info("Biến sanPhamData không được định nghĩa hoặc rỗng cho phần 12 sản phẩm khác.");
    }



    // ===== 4. Intel_core (sử dụng sanPham_Intel_Data) =====
    // Chỉ chạy nếu sanPham_Intel_Data được định nghĩa và có dữ liệu
    if (typeof sanPham_Intel_Data !== 'undefined' && sanPham_Intel_Data && sanPham_Intel_Data.length > 0) {
        const items3 = sanPham_Intel_Data.map((sp) => {
            const col = document.createElement('div');
            col.className = 'text-center col-md-3 mb-4';

            const card = document.createElement('div');
            card.className = 'card h-100 d-flex flex-column border  border-2 hover-border-thick text-success';

            const img = document.createElement('img');
            img.alt = sp.TenSP || 'Product image';
            img.src = sp.Img;
            img.className = 'card-img-top';

            const body = document.createElement('div');
            body.className = 'card-body d-flex flex-column';

            const title = document.createElement('h6');
            title.className = 'card-title';
            title.innerText = sp.TenSP;

            const price = document.createElement('p');
            price.className = 'card-text text-danger mb-1';
            price.innerText = sp.GiaHT;

            const original = document.createElement('p');
            original.className = 'card-text text-muted';
            original.innerHTML = `<s>${sp.GiaGoc}</s>`;

            const chillDiv = document.createElement('div');
            chillDiv.className = 'd-flex gap-2 mt-auto';

            const icon = document.createElement('i');
            icon.className = 'fas fa-shopping-cart align-self-center text-danger';

            const button = document.createElement('button');
            button.className = 'btn btn-danger flex-grow-1';
            button.type = 'submit';
            button.innerText = ' Thêm vào giỏ';
            button.addEventListener('click', function () {
                if (isLoggedIn === 'false') {
                    Swal.fire({
                        icon: 'warning',
                        title: `Bạn chưa đăng nhập`,
                        text: 'Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng!',
                        confirmButtonText: 'Đăng nhập'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = '/Home/DangNhap';
                        }
                    });
                }
                else {
                    addToCart(sp);
                    sendToCardSever(sp);
                }
            });
            button.setAttribute('data-masp', sp.Id);

            chillDiv.appendChild(icon);
            chillDiv.appendChild(button);

            body.appendChild(title);
            body.appendChild(price);
            body.appendChild(original);
            body.appendChild(chillDiv);

            card.appendChild(img);
            card.appendChild(body);
            col.appendChild(card);

            return col;
        });

        displayItemsToWrapper(items3, "Intel"); 
        displayItemsToWrapper(items3, "ryzen");

    } else {
        console.info("Biến sanPham_Intel_Data không được định nghĩa hoặc rỗng trên trang Intel_core.");
    }

    //===========chi tiet san pham
    const chiTietSanPham = document.getElementById("chiTietSanPham");
    if (chiTietSanPham) {
        chiTietSanPham.addEventListener('click', function () {
            if (isLoggedIn === "false") {
                Swal.fire({
                    icon: 'info',
                    title: 'Lưu ý',
                    text: 'Bạn chưa đăng nhập. Một số tính năng sẽ bị hạn chế.',
                    confirmButtonText: 'Đăng nhập'
                }).then(() => {
                    window.location.href = `/Home/DangNhap/${sp.Id}`;
                });
            } else {
                addToCart(window.sp);
                sendToCardSever(window.sp);
                alert(JSON.stringify(window.sp));
            }
        });
    }
    function sendToCardSever(sp) {
        
        fetch('/Home/ThongTinGioHang', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                IdSanPham: sp.Id,
                SoLuong: 1
            })
        })
            .then(response => {
                if (response.ok) {
                    showCustomMessage("Đã thêm sản phẩm vào giỏ hàng thành công!", true);
                } else if (response.status === 401) {
                    showCustomMessage("Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng.", false);

                    setTimeout(() => { window.location.href = '/Home/DangNhap'; }, 2000);
                } else {
                    showCustomMessage("Lỗi khi thêm sản phẩm vào giỏ hàng.", false);
                }
                return response.text();
            })
            .then(data => {
                console.log('Server Response:', data);
            })
            .catch(error => {
                console.error("Lỗi mạng hoặc lỗi khác:", error);
                showCustomMessage("Có lỗi xảy ra: " + error.message, false);
            });
    }

   



    //==========giohang
    function capNhatSoLuong(id, soLuong) {
        fetch('/Home/CapNhatSoLuong', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()  
            },
            body: JSON.stringify({
                Id: id,
                SoLuong: soLuong
            })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    if (soLuong === 0) {
                        
                        const card = document.querySelector(`[data-idgiohang='${sp.Id}']`)?.closest('.card');
                        if (card) card.remove();
                    }
                } else {
                    alert(data.message || "Có lỗi xảy ra khi cập nhật.");
                }
            });
    }

    //if (typeof sanPham_GioHang !== 'undefined' && sanPham_GioHang && sanPham_GioHang.length > 0) {
    //    const container = document.getElementById('container');

    //    sanPham_GioHang.forEach((sp) => {
    //        const card = document.createElement('div');
    //        card.className = 'card mb-3 bg-light';
    //        card.style.maxWidth = '100%';

    //        const row = document.createElement('div');
    //        row.className = 'row g-0';


    //        const colImg = document.createElement('div');
    //        colImg.className = 'col-md-3 d-flex align-items-center justify-content-center';
    //        const img = document.createElement('img');
    //        img.src = sp.Img;
    //        img.className = 'img-fluid rounded-start';
    //        img.style.maxHeight = '100px';
    //        colImg.appendChild(img);


    //        const colBody = document.createElement('div');
    //        colBody.className = 'col-md-6';

    //        const body = document.createElement('div');
    //        body.className = 'card-body';

    //        const title = document.createElement('h5');
    //        title.className = 'card-title';
    //        title.innerText = sp.TenSP;

    //        const giaHT = document.createElement('p');
    //        giaHT.className = 'cart-text mb-1';
    //        giaHT.innerHTML = `<strong>Giá hiện tại:</strong>${sp.GiaHT.toLocaleString()} ₫`;

    //        const giaGoc = document.createElement('p');
    //        giaGoc.className = 'card-text';
    //        giaGoc.innerHTML = `<small class="text-muted"><s>Giá gốc: ${sp.GiaGoc.toLocaleString()} ₫</s></small>`;

    //        const qtyControls = document.createElement('div');
    //        qtyControls.className = 'd-flex align-items-center mt-2';

    //        const btnMinus = document.createElement('button');
    //        btnMinus.className = 'btn btn-outline-secondary btn-sm';
    //        btnMinus.innerText = '-';
    //        btnMinus.onclick = function () {
    //            const input = this.parentElement.querySelector('input');
    //            let current = parseInt(input.value);
    //            if (current > 1) {
    //                const newQty = current - 1;
    //                input.value = newQty;
    //                document.querySelector(`.chon-sanpham[data-idgiohang='${sp.Id}']`).dataset.soluong = newQty;

    //                capNhatSoLuong(sp.Id, newQty);
    //            }
    //        };

    //        const inputQty = document.createElement('input');
    //        inputQty.type = 'text';
    //        inputQty.className = 'form-control text-center mx-2';
    //        inputQty.style.width = '60px';
    //        inputQty.id = `qty_${sp.ID}`;
    //        inputQty.value = sp.SoLuong;
    //        inputQty.readOnly = true;

    //        const btnPlus = document.createElement('button');
    //        btnPlus.className = 'btn btn-outline-secondary btn-sm';
    //        btnPlus.innerText = '+';
    //        btnPlus.onclick = function () {
    //            const input = this.parentElement.querySelector('input');
    //            let current = parseInt(input.value);
    //            const newQty = current + 1;
    //            input.value = newQty;
    //            document.querySelector(`.chon-sanpham[data-idgiohang='${sp.Id}']`).dataset.soluong = newQty;
    //            capNhatSoLuong(sp.Id, newQty);
    //        };

    //        qtyControls.appendChild(btnMinus);
    //        qtyControls.appendChild(inputQty);
    //        qtyControls.appendChild(btnPlus);

    //        body.appendChild(title);
    //        body.appendChild(giaHT);
    //        body.appendChild(giaGoc);
    //        body.appendChild(qtyControls);
    //        colBody.appendChild(body);


    //        const divCheck = document.createElement('div');
    //        divCheck.className = 'col-md-3 d-flex align-items-center justify-content-center';

    //        const inputCheck = document.createElement('input');
    //        inputCheck.type = 'checkbox';
    //        inputCheck.className = 'form-check-input chon-sanpham';
    //        inputCheck.style.width = '20px';
    //        inputCheck.style.height = '20px';
    //        inputCheck.style.border = '2px solid blue';
    //        inputCheck.dataset.idgiohang = sp.Id;
    //        inputCheck.dataset.giaht = sp.GiaHT;
    //        inputCheck.dataset.soluong = sp.SoLuong;


    //        divCheck.appendChild(inputCheck);


    //        row.appendChild(colImg);
    //        row.appendChild(colBody);
    //        row.appendChild(divCheck);

    //        card.appendChild(row);
    //        container.appendChild(card);
    //    });
    //}

    //function xuLyDatHang() {
    //    const checkboxes = document.querySelectorAll('.chon-sanpham');
    //    const tongTienElement = document.getElementById('tongTien');
    //    const btnDatHang = document.getElementById('btnDatHang');

    //    if (!checkboxes.length || !tongTienElement || !btnDatHang) {
    //        console.error("Không tìm thấy phần tử cần thiết để xử lý đặt hàng.");
    //        return;
    //    }

    //    function tinhTongTien() {
    //        let tong = 0;
    //        checkboxes.forEach(cb => {
    //            if (cb.checked) {
    //                const gia = parseFloat(cb.dataset.giaht);
    //                const id = cb.dataset.Id;
    //                const input = document.getElementById(`qty_${id}`);
    //                const soLuong = parseInt(input?.value||0);
    //                tong += gia * soLuong;
    //            }
    //        });
    //        tongTienElement.textContent = `Tổng tiền: ${tong.toLocaleString()} VNĐ`;
    //        return tong;
    //    }

    //    checkboxes.forEach(cb => {
    //        cb.addEventListener('change', tinhTongTien);
    //    });

    //    btnDatHang.addEventListener('click', function (e) {
    //        const selected = [];
    //        const tentinh = selectTinh.options[selectTinh.selectedIndex]?.text || 'Không rõ tỉnh';
    //        const tenxa = selectXa.options[selectXa.selectedIndex]?.text || 'Không rõ xã';
            

    //        checkboxes.forEach(cb => {
    //            if (cb.checked) {
    //                selected.push({
    //                    Id_TTGioHang: cb.dataset.idgiohang,
    //                    SoLuong: cb.dataset.soluong,
    //                    DiaChiNhanHang: tentinh + ' / ' + tenxa
    //                });
    //            }
    //        });
           


    //        if (selected.length === 0) {
    //            alert("Bạn chưa chọn sản phẩm nào!");
    //            return;
    //        }

          
    //        fetch('/Home/LichSuMuaHang', {
    //            method: 'POST',
    //            headers: { 'Content-Type': 'application/json' },
    //            body: JSON.stringify(selected)
    //        })
    //            .then(res => {
    //                if (res.ok) {
    //                    alert("Đặt hàng thành công!");

    //                    window.location.reload();
    //                } else {
    //                    alert("Lỗi khi đặt hàng!");
    //                }
    //            })
    //            .catch(err => {
    //                console.error("Lỗi mạng:", err);
    //            });
    //    });
    //}
    //xuLyDatHang();
    //function layThongTinDonHang() {
    //    const orderCode = "5ENLKKHD";
    //    fetch(`/Home/LayThongTinDonHang?orderCode=${orderCode}`)
    //        .then(res => {
    //            if (!res.ok) throw new Error("Lỗi gọi API server");
    //            return res.json();
    //        })
    //        .then(data => {
    //            console.log("Kết quả GHN:", data);
    //            alert(JSON.stringify(data));
    //        })
    //        .catch(err => {
    //            console.error("Lỗi khi gọi GHN:", err);
    //            alert("Lỗi khi gọi GHN");
    //        });
    //}

    //layThongTinDonHang();

    const token = "227dc3c7-50fb-11f0-ac24-c6fe55da14a6";

    // Load tỉnh
    $(document).ready(function () {
        fetch("https://online-gateway.ghn.vn/shiip/public-api/master-data/province", {
            headers: { Token: token }
        })
            .then(res => res.json())
            .then(data => {
                data.data.forEach(t => {
                    $('#selectTinh').append(`<option value="${t.ProvinceID}">${t.ProvinceName}</option>`);
                });
            });

        $('#selectTinh').on('change', function () {
            let provinceId = $(this).val();
            $('#selectQuan').html('<option>--Chọn quận--</option>');
            $('#selectXa').html('<option>--Chọn phường--</option>');

            fetch("https://online-gateway.ghn.vn/shiip/public-api/master-data/district", {
                method: "POST",
                headers: {
                    Token: token,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ province_id: parseInt(provinceId) })
            })
                .then(res => res.json())
                .then(data => {
                    data.data.forEach(q => {
                        $('#selectQuan').append(`<option value="${q.DistrictID}">${q.DistrictName}</option>`);
                    });
                });
        });

        $('#selectQuan').on('change', function () {
            let districtId = $(this).val();
            $('#selectXa').html('<option>--Chọn phường--</option>');

            fetch("https://online-gateway.ghn.vn/shiip/public-api/master-data/ward", {
                method: "POST",
                headers: {
                    Token: token,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ district_id: parseInt(districtId) })
            })
                .then(res => res.json())
                .then(data => {
                    data.data.forEach(xa => {
                        $('#selectXa').append(`<option value="${xa.WardCode}">${xa.WardName}</option>`);
                    });
                });
        });
    });

    // Render sản phẩm trong giỏ hàng
    function renderGioHang() {
        const container = document.getElementById("container");
        container.innerHTML = "";

        sanPham_GioHang.forEach(sp => {
            const card = document.createElement('div');
                    card.className = 'card mb-3 bg-light';
                    card.style.maxWidth = '100%';

                    const row = document.createElement('div');
                    row.className = 'row g-0';


                    const colImg = document.createElement('div');
                    colImg.className = 'col-md-3 d-flex align-items-center justify-content-center';
                    const img = document.createElement('img');
                    img.src = sp.Img;
                    img.className = 'img-fluid rounded-start';
                    img.style.maxHeight = '100px';
                    colImg.appendChild(img);


                    const colBody = document.createElement('div');
                    colBody.className = 'col-md-6';

                    const body = document.createElement('div');
                    body.className = 'card-body';

                    const title = document.createElement('h5');
                    title.className = 'card-title';
                    title.innerText = sp.TenSP;

                    const giaHT = document.createElement('p');
                    giaHT.className = 'cart-text mb-1';
                    giaHT.innerHTML = `<strong>Giá hiện tại:</strong>${sp.GiaHT.toLocaleString()} ₫`;

                    const giaGoc = document.createElement('p');
                    giaGoc.className = 'card-text';
                    giaGoc.innerHTML = `<small class="text-muted"><s>Giá gốc: ${sp.GiaGoc.toLocaleString()} ₫</s></small>`;

                    const qtyControls = document.createElement('div');
                    qtyControls.className = 'd-flex align-items-center mt-2';

                    const btnMinus = document.createElement('button');
                    btnMinus.className = 'btn btn-outline-secondary btn-sm';
                    btnMinus.innerText = '-';
                    btnMinus.onclick = function () {
                        const input = this.parentElement.querySelector('input');
                        let current = parseInt(input.value);
                        if (current > 0) {
                            const newQty = current - 1;
                            input.value = newQty;
                            document.querySelector(`.chon-sanpham[data-idgiohang='${sp.IdGioHang}']`).dataset.SoLuong = newQty;
                            //alert(sp.SoLuong + " " + newQty);
                            capNhatSoLuong(sp.IdSanPham, newQty);
                            
                            tinhTongTien();
                        }
                    };

                    const inputQty = document.createElement('input');
                    inputQty.type = 'text';
                    inputQty.className = 'form-control text-center mx-2';
                    inputQty.style.width = '60px';
                    inputQty.id = `qty_${sp.ID}`;
                    inputQty.value = sp.SoLuong;
                    inputQty.readOnly = true;

                    const btnPlus = document.createElement('button');
                    btnPlus.className = 'btn btn-outline-secondary btn-sm';
                    btnPlus.innerText = '+';
                    btnPlus.onclick = function () {
                        const input = this.parentElement.querySelector('input');
                        let current = parseInt(input.value);
                        const newQty = current + 1;
                        input.value = newQty;
                        
                        document.querySelector(`.chon-sanpham[data-idgiohang='${sp.IdGioHang}']`).dataset.SoLuong = newQty;
                        capNhatSoLuong(sp.IdSanPham, newQty);
                        alert(sp.IdSanPham + " " + newQty);
                        tinhTongTien();
                    };

                    qtyControls.appendChild(btnMinus);
                    qtyControls.appendChild(inputQty);
                    qtyControls.appendChild(btnPlus);

                    body.appendChild(title);
                    body.appendChild(giaHT);
                    body.appendChild(giaGoc);   
                    body.appendChild(qtyControls);
                    colBody.appendChild(body);


                    const divCheck = document.createElement('div');
                    divCheck.className = 'col-md-3 d-flex align-items-center justify-content-center';

                    const inputCheck = document.createElement('input');
                    inputCheck.type = 'checkbox';
                    inputCheck.className = 'form-check-input chon-sanpham';
                    inputCheck.style.width = '20px';
                    inputCheck.style.height = '20px';
                    inputCheck.style.border = '2px solid blue';
                    inputCheck.dataset.id = sp.IdSanPham;  
                    inputCheck.dataset.idgiohang = sp.IdGioHang;  
                    inputCheck.dataset.ten = sp.TenSP;
                    inputCheck.dataset.giaht = sp.GiaHT;
                    inputCheck.dataset.soluong = sp.SoLuong;

                    divCheck.appendChild(inputCheck);


                    row.appendChild(colImg);
                    row.appendChild(colBody);
                    row.appendChild(divCheck);

                    card.appendChild(row);
                    container.appendChild(card);
        });

         
        document.querySelectorAll(".chon-sanpham").forEach(cb => {
            cb.addEventListener("change", tinhTongTien);
        });

        tinhTongTien();
    }

    // Tính tổng tiền khi chọn sản phẩm
    function tinhTongTien() {
        let tong = 0;

        document.querySelectorAll(".chon-sanpham").forEach(cb => {
            if (cb.checked) {
                const gia = parseInt(cb.dataset.giaht);
                const soLuong = parseInt(cb.dataset.soluong);
                tong += gia * soLuong;
            }
        });

         
        const tongTienElement = document.getElementById("tongTien");
        if (tongTienElement) {
            tongTienElement.innerText = `Tổng tiền: ${tong.toLocaleString()} VNĐ`;
        }

        return tong;  
    }



    const phuongThucThanhToan = document.querySelector('input[name="phuongThucThanhToan"]:checked')?.value;
    console.log("Phương thức thanh toán:", phuongThucThanhToan);

    // Xử lý khi nhấn nút đặt hàng
    const check = document.getElementById("btnDatHang");
    if (check) {
        document.getElementById("btnDatHang").addEventListener("click", function () {
            const ten = document.getElementById("tenNguoiNhan").value;
            const sdt = document.getElementById("sdt").value;
            const diaChiChiTiet = document.getElementById("diaChi").value || "";
            const tenTinh = document.getElementById("selectTinh").selectedOptions[0]?.text || "";
            const tenQuan = document.getElementById("selectQuan").selectedOptions[0]?.text || "";
            const tenXa = document.getElementById("selectXa").selectedOptions[0]?.text || "";
            const fullDiaChi = `${diaChiChiTiet}, ${tenXa}, ${tenQuan}, ${tenTinh}`;
            const districtId = parseInt(document.getElementById("selectQuan").value);
            const wardCode = document.getElementById("selectXa").value;

            const phuongThucThanhToan = document.querySelector('input[name="phuongThucThanhToan"]:checked')?.value;
            console.log("Phương thức thanh toán:", phuongThucThanhToan);

            const selectedSP = [];

            document.querySelectorAll(".chon-sanpham").forEach(cb => {
                if (cb.checked) {
                    selectedSP.push({
                        IdSanPham: parseInt(cb.dataset.id),
                        Id_TTGioHang: parseInt(cb.dataset.idgiohang),
                        TenSanPham: cb.dataset.ten,
                        SoLuong: parseInt(cb.dataset.soluong),
                        Gia: parseInt(cb.dataset.giaht)
                    });
                }
            });

            if (!ten || !sdt || !diaChiChiTiet || !districtId || !wardCode || selectedSP.length === 0 || !phuongThucThanhToan) {
                alert("Vui lòng nhập đầy đủ thông tin và chọn phương thức thanh toán!");
                return;
            }

            const donHang = {
                TenNguoiNhan: ten,
                SDT: sdt,
                DiaChi: fullDiaChi,
                DistrictId: districtId,
                WardCode: wardCode,
                TongTien: tinhTongTien(),
                SanPhams: selectedSP,
                PhuongThucThanhToan: phuongThucThanhToan
            };

            fetch('/Home/LichSuMuaHang', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(donHang)
            })
                .then(res => {
                    if (!res.ok) throw new Error("Server error");
                    return res.json();
                })
                .then(data => {
                    if (data.success) {
                        if (phuongThucThanhToan === "COD") {
                            // Nếu COD: hiển thị form/chat đặt hàng thành công
                            Swal.fire({
                                icon: 'success',
                                title: 'Đặt hàng thành công!',
                                html: `<p>Đơn hàng của bạn đã được ghi nhận.</p>
                                   <p><strong>Tổng tiền:</strong> ${tinhTongTien().toLocaleString()} VND</p>`,
                                confirmButtonText: 'OK'
                            });
                        } else {
                            // Nếu VNPAY hoặc MOMO: gửi email + chuyển hướng
                            Swal.fire({
                                icon: 'info',
                                title: 'Vui lòng thanh toán',
                                text: data.message || 'Bạn sẽ được chuyển hướng đến trang thanh toán.',
                                showConfirmButton: false,
                                timer: 2000
                            });

                            if (data.redirectUrl) {
                                setTimeout(() => {
                                    window.location.href = data.redirectUrl;
                                }, 2000);
                            }
                        }
                    } else {
                        alert("Đặt hàng thất bại! " + (data.message || ""));
                    }
                })
                .catch(err => {
                    console.error("Lỗi mạng:", err);
                    alert("Có lỗi xảy ra khi đặt hàng. Vui lòng thử lại.");
                });
        });

        renderGioHang();
    }

    


    function showCustomMessage(message, isSuccess = true) {
        const messageBox = document.getElementById('custom-message-box');
        if (!messageBox) {
            console.error('Element with ID "custom-message-box" not found. Cannot display message.');

            if (isSuccess) {
                console.log(message);
            } else {
                console.error(message);
            }
            return;
        }

        messageBox.innerText = message;
        messageBox.style.display = 'block';
        messageBox.className = isSuccess ? 'alert alert-success' : 'alert alert-danger';


        setTimeout(() => {
            messageBox.style.display = 'none';
        }, 3000);
    }


    document.getElementById("btnLichSu").addEventListener("click", function () {
        // Gọi API lấy lịch sử mua hàng
        fetch('/Home/GetLichSuMuaHang', {
            method: 'GET'
        })
            .then(res => res.json())
            .then(data => {
                const lichSuContainer = document.getElementById("lichSuContent");
                lichSuContainer.innerHTML = "";

                if (data.length === 0) {
                    lichSuContainer.innerHTML = "<p class='text-center text-muted'>Chưa có đơn hàng nào.</p>";
                    return;
                }

                data.forEach(don => {
                    const div = document.createElement("div");
                    div.className = "border-bottom pb-2 mb-2";

                    div.innerHTML = `
                    <p><strong>Mã đơn:</strong> ${don.MaDon}</p>
                    <p><strong>Ngày đặt:</strong> ${new Date(don.NgayDat).toLocaleDateString()}</p>
                    <p><strong>Tổng tiền:</strong> ${don.TongTien.toLocaleString()} ₫</p>
                    <p><strong>Trạng thái:</strong> ${don.TrangThai}</p>
                `;

                    lichSuContainer.appendChild(div);
                });
            })
            .catch(err => {
                document.getElementById("lichSuContent").innerHTML =
                    "<p class='text-danger text-center'>Lỗi khi tải lịch sử mua hàng.</p>";
                console.error(err);
            });

       
        const modal = new bootstrap.Modal(document.getElementById("lichSuModal"));
        modal.show();
    });


    


    //function loadTinhVaXa() {
    //    let tinhXaData = [];

    //    fetch('/Home/GetTinhVaXa')
    //        .then(response => response.json())
    //        .then(data => {
    //            tinhXaData = data;

    //            const selectTinh = document.getElementById('selectTinh');
    //            const selectXa = document.getElementById('selectXa');


    //            data.forEach(tinh => {
    //                const option = document.createElement('option');
    //                option.value = tinh.Id;
    //                option.textContent = tinh.TenTinh;
    //                selectTinh.appendChild(option);
    //            });


    //            selectTinh.addEventListener('change', function () {
    //                const selectedId = parseInt(this.value);
    //                const selectedTinh = tinhXaData.find(t => t.Id === selectedId);


    //                selectXa.innerHTML = '<option value="">-- Chọn xã --</option>';

    //                if (selectedTinh && selectedTinh.XaPhuongs.length > 0) {
    //                    selectedTinh.XaPhuongs.forEach(xa => {
    //                        const option = document.createElement('option');
    //                        option.value = xa.Id;
    //                        option.textContent = xa.TenXaPhuong;
    //                        selectXa.appendChild(option);
    //                    });
    //                }
    //            });
    //        })
    //        .catch(error => console.error('Lỗi khi lấy tỉnh/xã:', error));

    //}
    //loadTinhVaXa();

    


     

}); 
