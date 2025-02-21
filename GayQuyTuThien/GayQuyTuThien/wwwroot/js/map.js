const milkProvince = [{ "_id": "65f415a1470239f4262311c7", "milk": "0", "child": "0", "address": "Cao Bằng", "createdAt": "2024-03-21T11:29:26.043Z", "updatedAt": "2024-03-21T11:29:26.043Z", "slug": "cao-bang" }, { "_id": "65f415a2470239f4262311c9", "milk": "0", "child": "0", "address": "Bắc Kạn", "createdAt": "2024-03-21T11:29:26.046Z", "updatedAt": "2024-03-21T11:29:26.046Z", "slug": "bac-kan" }, { "_id": "65f415a2470239f4262311cb", "milk": "0", "child": "0", "address": " Lào Cai", "createdAt": "2024-03-21T11:29:26.047Z", "updatedAt": "2024-03-21T11:29:26.047Z", "slug": "lao-cai" }, { "_id": "65f415a2470239f4262311cd", "milk": "0", "child": "0", "address": "Yên Bái", "createdAt": "2024-03-21T11:29:26.049Z", "updatedAt": "2024-03-21T11:29:26.049Z", "slug": "yen-bai" }, { "_id": "65f415a2470239f4262311cf", "milk": "0", "child": "0", "address": "Điện Biên", "createdAt": "2024-03-21T11:29:26.051Z", "updatedAt": "2024-03-21T11:29:26.051Z", "slug": "dien-bien" }, { "_id": "65f415a2470239f4262311d1", "milk": "0", "child": "0", "address": "Bắc Giang", "createdAt": "2024-03-21T11:29:26.052Z", "updatedAt": "2024-03-21T11:29:26.052Z", "slug": "bac-giang" }, { "_id": "65f415a2470239f4262311d3", "milk": "0", "child": "0", "address": "Thái Nguyên", "createdAt": "2024-03-21T11:29:26.054Z", "updatedAt": "2024-03-21T11:29:26.054Z", "slug": "thai-nguyen" }, { "_id": "65f415a2470239f4262311d5", "milk": "0", "child": "0", "address": "Hải Dương", "createdAt": "2024-03-21T11:29:26.060Z", "updatedAt": "2024-03-21T11:29:26.060Z", "slug": "hai-duong" }, { "_id": "65f415a2470239f4262311d7", "milk": "0", "child": "0", "address": "Hà Nội", "createdAt": "2024-03-21T11:29:26.062Z", "updatedAt": "2024-03-21T11:29:26.062Z", "slug": "ha-noi" }, { "_id": "65f415a2470239f4262311d9", "milk": "0", "child": "0", "address": "Bắc Ninh", "createdAt": "2024-03-21T11:29:26.064Z", "updatedAt": "2024-03-21T11:29:26.064Z", "slug": "bac-ninh" }];
if (milkProvince) {
    let html = ""
    milkProvince.forEach(element => {
        html += `<div class="province ${element.slug} ${(element.milk != '0' || element.child != '0') ? 'active' : ''}">
                                <div class="name">
                                    <img loading="lazy" src="/image/name-bg.png" alt="">
                                    <span class="province-name text-nowrap"><b>${element.address}</b></span>
                                </div>
                                <div class="position text-center">
                                    <img loading="lazy" src="/image/position-icon.png" alt="">
                                </div>

                            </div>`
    });
    $("#provinces").html(html)
}
// })
// <div class="detail-btn text-center">
//                             <button class="btn" data-bs-toggle="modal" data-bs-target="#positionModal" onclick="initPosition('${element.address}','${element.milk}','${element.child}')">Chi tiết</button>
//                         </div>
function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    return parts.join(",");
}
function initPosition(address, milk, child) {
    $("#positionModal .province_name").text(address)
    $("#positionModal #count_milk").text(numberWithCommas(milk))
    $("#positionModal #count_child").text(numberWithCommas(child))
}
const slider = document.querySelector('#map-content');
let isDown = false;
let startX;
let scrollLeft;

slider.addEventListener('mousedown', (e) => {
    isDown = true;
    slider.classList.add('active');
    startX = e.pageX - slider.offsetLeft;
    scrollLeft = slider.scrollLeft;
});

slider.addEventListener('mouseleave', () => {
    isDown = false;
    slider.classList.remove('active');
});

slider.addEventListener('mouseup', () => {
    isDown = false;
    slider.classList.remove('active');
});

slider.addEventListener('mousemove', (e) => {
    if (!isDown) return;
    e.preventDefault();
    const x = e.pageX - slider.offsetLeft;
    const walk = (x - startX) * 1.5; // Có thể điều chỉnh tỷ lệ này để thay đổi tốc độ cuộn
    slider.scrollLeft = scrollLeft - walk;
});