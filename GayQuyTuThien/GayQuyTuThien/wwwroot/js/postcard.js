function generatorPostcard(e) {

    $(e).prop("disabled", false);
    const userName = $('#postcardModal #username').val()

    if (!userName || userName.length > 13) {
        $('#postcardModal .modal-body .sum-character .alert-name').removeClass('invisible')
        return
    }
    $(e).attr("disabled", true);

    setTimeout(() => {
        $("#postcardModal .btn-create-postcard").attr('disabled', false)
    }, 3000);
    var canvas = document.getElementById('postcardCanvas');
    var ctx = canvas.getContext('2d');
    var img = new Image();
    var img2 = new Image();
    img2.src = "image/postcard_bg_2222.png";
    img.crossOrigin = "anonymous";

    const gender = $('#postcardModal .gender-input:checked').val()

    let prefixs = [
        ' Cùng em đến trường + Ươm mầm đất nước + Hành trang đến trường + Ngàn lời yêu thương',
        ' Mỗi sáng tinh sương + Gà rừng mới thức + Em đã băng rừng + Đi tìm con chữ',
        ' Mong muốn là nụ cười của bé + Cánh chim non mạnh khỏe lớn khôn + Vô tư trong sáng tâm hồn + Thiện lương hạt giống ta dồn công chăm',
        ' Greenmilk Việt Nam + Yêu thương dạt dào + Đời thêm ngọt ngào + Hạnh phúc biết bao',
        ' Greenmilk Việt Nam + Yêu thương dạt dào + Đời thêm ngọt ngào + Hạnh phúc biết bao',
        ' Đồ dùng học tập và sách vở mới + cô chú gửi tặng đẹp lắm ạ, + chúng con hứa sẽ học tập chăm ngoan + để ko phụ tình yêu thương từ cô chú ạ.',
        ' Cặp sách cô chú gửi đẹp ơi là đẹp + Tình yêu thương của cô chú thì to ơi là to',
        ' Hôm nay con vừa được điểm 10 vừa biết + sẽ nhận được quà từ cô chú'


    ];
    let thanksText = gender == 0 ? "Con cảm ơn cô" : "Con cảm ơn chú";

    // Mảng chứa hậu tố của câu chúc cần chèn
    let suffixes = [
        'đã gửi yêu thương + cùng chúng con đến trường ạ',
        'nhiều lắm ạ.',
        'đã gửi quà tới chúng con ạ!',
        'đã yêu thương và tặng quà cho chúng con, + mong cô chú luôn luôn mạnh khỏe và bình an ạ',
        'đã yêu thương con ạ',
        'nhiều ạ!',
        'đã giúp chúng con có thêm + nhiều niềm vui mỗi ngày đến trường.'
    ];
    var randomPreIndex = randomIndex(prefixs);
    var randomSufindex = randomIndex(suffixes);
    let prefix = prefixs[randomPreIndex];
    let suffix = suffixes[randomSufindex];

    // Tạo một đối tượng FontFace
    var font = new FontFace('Pecita', 'url(../fonts/Pecita.ttf)');
    document.fonts.add(font);

    img.onload = function () {
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height - 300); // Vẽ hình ảnh trên canvas
        ctx.drawImage(img2, 0, 0, canvas.width, canvas.height);
        ctx.font = '37px Pecita';
        ctx.textBaseline = "middle";
        var PY = 700;
        function drawWrappedText(context, text, x, y, lineHeight, thanksText, userName, suffix) {
            var words = text.split('+');
            context.textAlign = "center"; // Đặt căn giữa cho trục x
            context.fillStyle = "black";
            var offsetY = 0;

            for (var i = 0; i < words.length; i++) {
                
                var word = words[i];
                context.fillText(word.trim(), x, y + offsetY);
                
                offsetY += lineHeight;
            }
            var totalLines = words.length;
            var totalHeight = totalLines * lineHeight;
            
            PY += totalHeight;
            var parts = [
                { text: thanksText, color: "black" },
                { text: userName, color: "red" },
            ];
            var totalWidth = 0;
            parts.forEach(function (part) {
                totalWidth += context.measureText(part.text).width;
            });

            // Vẽ phần tử text màu đen
            context.textAlign = "right";
            context.fillStyle = parts[0].color; // Màu của phần tử text đầu tiên
            context.fillText(parts[0].text, x, PY);

            // Vẽ phần tử text màu đỏ
            context.textAlign = "left"; // Đặt lại căn giữa cho phần tử text màu đỏ
            context.fillStyle = parts[1].color; // Màu của phần tử text thứ hai
            context.fillText(parts[1].text, x + 10, PY); // Vẽ văn bản màu đỏ

            PY += lineHeight;
            var lastwords = suffix.split('+');
            context.textAlign = "center"; // Đặt căn giữa cho trục x
            context.fillStyle = "black";
            var offsetY2 = 0;

            for (var i = 0; i < lastwords.length; i++) {

                var lastword = lastwords[i];
                context.fillText(lastword.trim(), x, PY + offsetY2);

                offsetY2 += lineHeight;
            }

        }


        // Sử dụng hàm drawWrappedText để vẽ văn bản
        drawWrappedText(ctx, prefix, canvas.width / 2, PY, 37, thanksText, userName, suffix );
        $('#postcardModal').modal('hide')
        if (GVs.isIOS()) {
            $('#ios-image-modal .modal-body img').attr('src', canvas.toDataURL())
            $('#ios-image-modal').modal('show')
        } else {

            $('#resultModal #img-result').attr('src', canvas.toDataURL())
            $('#resultModal').modal('show')
        }

    };

    // Lắng nghe sự kiện load
    font.load().then(function (loadedFont) {
        postCreateCard(userName, gender, img)
    }).catch(function (error) {
        img.src = '/image/postcard/image-' + (postcard + 1) + '.jpg';
        // Font không thể tải
        console.error('Font load error:', error);
    });

}
function isFacebookApp() {
    var ua = navigator.userAgent || navigator.vendor || window.opera;
    return (ua.indexOf("FBAN") > -1) || (ua.indexOf("FBAV") > -1) && ua.toLowerCase().indexOf("android") > -1;
}
function downloadPostCard() {
    if (isFacebookApp()) {
        $('#resultModal').modal('hide')
        $('#androidModel').modal('show')
    } else {
        var canvas = document.getElementById('postcardCanvas');
        var dataURL = canvas.toDataURL('image/png');
        var a = document.createElement('a');
        a.href = dataURL;
        a.download = 'Greenmilk - Lời tri ân.png'; // Tên file khi tải xuống
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    }
}

function onInputNameChange(e) {
    if (e.target.value.length > 0) {
        $('#postcardModal .modal-body .sum-character .alert-name').addClass('invisible')
    }
    $('#postcardModal #count-name').html(e.target.value.length)
}
function randomIndex(array) {
    var randomIndex = Math.floor(Math.random() * array.length);
    return randomIndex;
}
function postCreateCard(username, gender, img) {
    $.ajax({
        url: GVs.CREATE_CARD,
        method: 'POST',
        data: {
            username,
            gender
        }
    }).done(function (response) {
        img.src = response.randomPost.guid
        var contentHtml = document.getElementById('contentHTML');
        if (response.contentHTML.length === 1) {
            contentHtml.innerHTML = response.contentHTML[0].content;
        }
    });
}

window.addEventListener('load', () => {
    $('#resultModal').on('hidden.bs.modal', event => {
        var canvas = document.getElementById('postcardCanvas');
        var ctx = canvas.getContext('2d');
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        $('#resultModal #img-result').attr('src', '')
    })
    $('#postcardModal').on('hidden.bs.modal', event => {
        $('#postcardModal #username').val('')
        $('#postcardModal #count-name').html('0')
        $('#postcardModal .modal-body .sum-character .alert-name').addClass('invisible')
        $('#postcardModal .gender-input').prop('checked', false)
    })
    $('#ios-image-modal').on('hidden.bs.modal', event => {
        $('#ios-image-modal .modal-body img').attr('src', '')
    })
})

    function removeDisable() {
    $("#postcardModal .btn-create-postcard").prop('disabled', false)
}
function copyClipboard() {
    var element = document.getElementById('contentHTML');

    // Create a temporary div element
    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = element.innerHTML;

    // Add the temporary div to the DOM
    document.body.appendChild(tempDiv);

    // Create a range and select the content
    const range = document.createRange();
    range.selectNodeContents(tempDiv);
    const selection = window.getSelection();
    selection.removeAllRanges();
    selection.addRange(range);

    // Copy the selected content to the clipboard
    document.execCommand('copy');

    // Clear the selection
    selection.removeAllRanges();

    // Remove the temporary div from the DOM
    document.body.removeChild(tempDiv);

    toastr.success('Sao chép thành công')

}