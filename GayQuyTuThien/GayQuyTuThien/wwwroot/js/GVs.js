class GVs {
    static setting = (url, method = "GET", data = {}) => {
        return {
            url: url,
            method: method,
            headers: {
                "Content-Type": "application/json"
            },
            data: JSON.stringify(data),
        }
    }
    static getCookie = (cname) => {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(";");
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == " ") {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length).replace(/"/g, '').replace(/^j:/, '');
            }
        }
        return "";
    }

    static getIdUrl = () => {
        return window.location.pathname.split('/').pop()
    }

    static LOGIN = "/api/login"
    static MILK = '/api/milk'
    static CREATE_CARD = '/Home/SubmitForm'
    static MILKPROVINCE = '/api/milk-province'
    static isIOS() {
        return [
            'iPad Simulator',
            'iPhone Simulator',
            'iPod Simulator',
            'iPad',
            'iPhone',
            'iPod'
        ].includes(navigator.platform)
            || (navigator.userAgent.includes("Mac") && "ontouchend" in document);
    }
}