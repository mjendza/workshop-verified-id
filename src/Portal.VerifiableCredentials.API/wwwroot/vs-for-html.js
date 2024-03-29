(function () {
    
    var setIntervalIdToCheckVcStatus = null;
    var buttonNextIdForB2C = "next";
    var checkStatus = 3000;
   
    var sessionKey = getRandomInt(9000000)
    function getRandomInt(max) {
        return Math.floor(Math.random() * max);
    }
    function checkDeviceTypeAndHideNotNeeded(){
        if (/iPhone/i.test(navigator.userAgent) || /Android/i.test(navigator.userAgent)) {
            $('.content-web').addClass('d-none');
        }else{
            $('.content-mobile').addClass('d-none');
        }
    }
    function initPresentationView(){
        
    }
    function runPresentation(){
        if(window.location.href.indexOf("issue") > -1) {
                      
        }
        if(window.location.href.indexOf("present") > -1) {
            presentVc();          
        }
    }
    function getConfiguration(){
        var fullRootHost = $(location).attr('protocol')+"//"+$(location).attr('hostname');
        
        if(window.location.href.indexOf("issue") > -1 && window.location.href.indexOf("demo-face") > -1) {
            $("#sign-in").click(issueVcWithDisplayClaimsForTheUser);
            return {
                apiPresentationPrefix: `${fullRootHost}/api/presentation`,
                apiIssuancePrefix: `${fullRootHost}/api/issuance`,
                showVcClaims: true,
                type: "face-issue",
                host: `${fullRootHost}/api/`
            }
        }
        if(window.location.href.indexOf("present") > -1 && window.location.href.indexOf("demo-face") > -1) {
            initPresentationView();
            return {
                apiPresentationPrefix: `${fullRootHost}/api/presentation`,
                apiIssuancePrefix: `${fullRootHost}/api/issuance`,
                showVcClaims: true,
                type: `face-present`,
                host: `${fullRootHost}/api/`
            }
        }
        
        if(window.location.href.indexOf("issue") > -1) {
            return {
                apiPresentationPrefix: `${fullRootHost}/api/presentation`,
                apiIssuancePrefix: `${fullRootHost}/api/issuance`,
                showVcClaims: true,
                type: "basic-issue",
                host: `${fullRootHost}/api/`
            }
        }
        if(window.location.href.indexOf("present") > -1) {
            initPresentationView();
            return {
                apiPresentationPrefix: `${fullRootHost}/api/presentation`,
                apiIssuancePrefix: `${fullRootHost}/api/issuance`,
                showVcClaims: true,
                type: `basic-present`,
                host: `${fullRootHost}/api/`
            }
        }
        return undefined;
    }
    var config = getConfiguration();
    
    
    
    var respIssuanceReq = null;
    function issueVcWithDisplayClaimsForTheUser(){
        // add self asserted claims, if any 
        var request = {};
        if (selfAssertedClaims) {
            var idx;           
            for (idx = 0; idx < Object.keys(selfAssertedClaims).length; idx++) {
                var id = idx + 1;               
                request[Object.keys(selfAssertedClaims)[idx]] = document.getElementById('entry' + id).value;                
            }
        }
        
        fetch(`${config.apiIssuancePrefix}/request`, {
            body: JSON.stringify(request),
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        })
            .then(response => response.text())
            .catch(error => document.getElementById("message").innerHTML = error)
            .then(response => {
                if (response.length <= 0) {
                    return;
                }
                console.log(response)
                respIssuanceReq = JSON.parse(response);
                if(document.getElementById('open-authenticator-app')!=null){
                    document.getElementById('open-authenticator-app').href = respIssuanceReq.url;
                }                
                
                if (respIssuanceReq.error_description) {
                    document.getElementById("message").innerHTML = respIssuanceReq.error_description;
                    document.getElementById('message-wrapper').style.display = "block";
                    respIssuanceReq = null;
                } else {
                    displayQrCode(respIssuanceReq.url, respIssuanceReq.pin);
                    if(setIntervalIdToCheckVcStatus!=null){
                        clearInterval(setIntervalIdToCheckVcStatus);
                    }
                    setIntervalIdToCheckVcStatus = setInterval(function () {
                        checkVcStatus(respIssuanceReq, config.apiIssuancePrefix, "issue")
                    }, checkStatus);
                }
            })
    }
    
    var selfAssertedClaims;
    var credentialType;
    function showClaimsFromSettings(selfAssertedClaims){
        if (Object.keys(selfAssertedClaims).length > 0) {
            var idx = 1;
            var html = "";
            for (idx = 0; idx < Object.keys(selfAssertedClaims).length; idx++) {
                var id = idx + 1;
                html += "<div class=\"entry-item row\" id=\"selfAssertedClaims" + id + "\">"
                    + "<label class=\"col-6 w-100 \" id=\"label-entry" + id + "\" for=\"entry" + id + "\">" + Object.keys(selfAssertedClaims)[idx] + "</label>"
                    + "<input class=\"col-6 no-edit w-100 \" type=\"text\" id=\"entry" + id + "\" name=\"entry" + id + "\" pattern=\"\" placeholder=\"" + selfAssertedClaims[Object.keys(selfAssertedClaims)[idx]] + "\" value=\"\" disabled></div>";
            }
            document.getElementById('selfAssertedClaims').innerHTML = html + "<p/>";
            document.getElementById('selfAssertedClaims').style.display = "block";
        }
    }


    function initView(){
        checkDeviceTypeAndHideNotNeeded();    
        $(".back-to-index").click(backToMainPage);   
        
        fetch(`${config.host}website/issuer/settings`,{
            headers: {
                
            }        
        })
            .then(response => response.text())
            .catch(error => document.getElementById("message").innerHTML = error)
            .then(response => {
                if (response.length <= 0) {
                    return;
                }
                var data = JSON.parse(response);
                document.body.style.backgroundColor = data.displayCard.backgroundColor;
                if(document.getElementById('vc-logo')){
                    document.getElementById('vc-logo').src = data.displayCard.logo.uri;
                }

                credentialType = data.credentialType;

                // open up text fields for self asserted claims, if any
                selfAssertedClaims = null;
                if (data.selfAssertedClaims) {
                    selfAssertedClaims = data.selfAssertedClaims;
                    if( config.showVcClaims===true){
                        showClaimsFromSettings(selfAssertedClaims);
                    }
                }
                document.querySelector("#inp").addEventListener("change", readFile);
                if(config.type ==="basic-issue"){
                    issueVcWithDisplayClaimsForTheUser();
                }
            });
    }
    function readFile() {
        if (!this.files || !this.files[0]) return;
        const FR = new FileReader();
        FR.addEventListener("load", function(evt) {            
            document.querySelector("#img").src = evt.target.result;
            document.getElementById('entry1').value = removeBeginningOfTheBase64DataPhoto(evt.target.result);
            if(document.querySelector("#img").clientWidth>200 || document.querySelector("#img").clientHeight>200){
                document.querySelector("#img").src = "";
                document.getElementById('entry1').value = "";                
            }
        });
        FR.readAsDataURL(this.files[0]);
    } 
    function removeBeginningOfTheBase64DataPhoto(photo){
        return photo.replace("data:image/jpeg;base64,", "");
    }
    
    function presentVc(){
        const urlParams = new URLSearchParams(window.location.search);
        const faceCheckFromQueryParam = urlParams.get('faceCheckEnabled');
        var request = {
            faceCheckEnabled: faceCheckFromQueryParam==='1',
            bankOperation: config.bankOperation
        };
        
        fetch(`${config.apiPresentationPrefix}/request`, {
            body: JSON.stringify(request),
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        })
            .then(response => response.text())

            .then(response => {
                if (response.length > 0) {                    
                    processPresentationResponse(response, config.apiPresentationPrefix);
                }
            });
    }
    
    function processPresentationResponse(response, statusPrefix){
        console.log(response)
        respPresReq = JSON.parse(response);
        document.getElementById('open-authenticator-app').href = respPresReq.url;

        if (/Android/i.test(navigator.userAgent)) {
            console.log(`Android device! Using deep link (${respPresReq.url}).`);
        }

        if (respPresReq.error_description) {
            respPresReq = null;
            return;
        } else {
           
            displayQrCode(respPresReq.url, respPresReq.pin);
            requestId = respPresReq.id;
            if(setIntervalIdToCheckVcStatus!=null){
                clearInterval(setIntervalIdToCheckVcStatus);
            }
            setIntervalIdToCheckVcStatus = setInterval(function () {
                checkVcStatus(respPresReq, statusPrefix, "present")
            }, checkStatus);
            }
        }
    function checkVcStatus(responseObject, prefix, type){
            fetch(`${prefix}/status?id=${responseObject.id}`,{
                headers: {
                    'x-session-key': `${sessionKey}`,
                    'x-type': `${config.type}`,
                    'x-product-session-key': `${config.productSessionKey}`,
                    'x-payment': `${config.payment}`
                }
            })
                .then(response => response.text())
                .catch(error => document.getElementById("message").innerHTML = error)
                .then(response => {
                    if (response.length <= 0) {
                        return;
                    }
                    console.log(response)
                    respMsg = JSON.parse(response);
                    //document.getElementById('message-wrapper').style.display = "block";
                    document.getElementById('qr-text').style.display = "none";
                    //document.getElementById('portal-link').style.display = "block";
                    
                    // respMsg.status == 1 -> QR Code scanned
                    if (respMsg.status == 1) {
                        //document.getElementById('message').innerHTML = respMsg.message;
                        document.getElementById("qr-code").style.opacity = "0.1";
                        //clearInterval(checkStatus);
                    }
                    // respMsg.status == 2 -> VC issued
                    if (respMsg.status == 2) {
                        document.getElementById('qr-code-frame').style.display = "none";
                        document.getElementById('qr-code').style.display = "none";
                        //document.getElementById('message').innerHTML = respMsg.message;
                        if(!(config.type.indexOf("b2c")>-1) && type==="present") {
                            displayPresentedVc(responseObject.id);
                        }
                        if(config.type.indexOf("b2c")>-1){
                            document.getElementById(buttonNextIdForB2C).click();
                            return;
                        }
                        clearInterval(setIntervalIdToCheckVcStatus);
                    }
                    // respMsg.status == 99 -> VC issueance failed
                    if (respMsg.status == 99) {
                        document.getElementById('qr-code-frame').style.display = "none";
                        document.getElementById('qr-code').style.display = "none";
                        //document.getElementById('message').innerHTML = respMsg.message;
                        //document.getElementById('message').style.textColor = "red";
                        clearInterval(setIntervalIdToCheckVcStatus);
                    }

                });
        }
        
    function processIssuanceRequest(){
        
        fetch(`${config.apiIssuancePrefix}/request`, {
            method: "POST",
            body: JSON.stringify({productSessionId: config.productSessionKey}),
            headers: {
                "Content-Type": "application/json",
            }
        })
            .then(response => response.text())
            .catch(error => document.getElementById("message").innerHTML = error)
            .then(response => {
                if (response.length <= 0) {
                    return;
                }
                console.log(response)
                respIssuanceReq = JSON.parse(response);
                if (respIssuanceReq.error_description) {
                    document.getElementById("message").innerHTML = respIssuanceReq.error_description;
                    document.getElementById('message-wrapper').style.display = "block";
                    respIssuanceReq = null;
                } else {
                    displayQrCode(respIssuanceReq.url, respIssuanceReq.pin);
                    
                    if(setIntervalIdToCheckVcStatus!=null){
                        clearInterval(setIntervalIdToCheckVcStatus);
                    }
                    setIntervalIdToCheckVcStatus = setInterval(function () {
                        checkVcStatus(respIssuanceReq, config.apiIssuancePrefix, "issue")
                    }, checkStatus);
                }
            })
    }
    
    function displayPresentedVc(requestId) {
        fetch(`${config.apiPresentationPrefix}/response-html?id=` + requestId, {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json"
            },
            redirect: "follow",
            referrerPolicy: "no-referrer",
            body: JSON.stringify({id: requestId}),
        })
            .then(response => response.text())

            .then(response => {
                if (response.length > 0) {
                    console.log(`DEBUG - with pure Entra VC API result for data`);
                    console.log(response);
                    respPresReq = JSON.parse(response);
                   
                    if(config.type.indexOf("present") > -1){
                        document.getElementById('summary').style.display = "";
                        
                        document.getElementById('init-page-container').style.display = "none";                        
                        document.getElementById('display-name').textContent = respPresReq.displayName;
                        document.getElementById('vc-iss').textContent = respPresReq.vcIss;
                        $('#vc-iss-name').html(respPresReq.vcIss);
                        document.getElementById('vc-sub').textContent = respPresReq.vcSub;
                        document.getElementById('job-title').textContent = respPresReq.jobTitle;
                        document.getElementById('subject-link').href = "https://identity.foundation/ion/explorer/?did=" + respPresReq.vcSub;

                        document.getElementById('face-check-result').textContent = respPresReq.faceCheckMatchConfidenceScore;
                        document
                            .getElementById('photo')
                            .src = `data:image/jpg;base64,${respPresReq.photo}`;
                        
                    }else{
                        console.log("not supported presentation type - for now only console log");
                        console.log(respPresReq);                        
                    }                                   
                }
            });
    }   
    function textFromBase64(base64) {
        return atob(base64);
    }
    function displayQrCode(qrCodeUrl, pin){
        var svgNode = QRCode({
            msg: qrCodeUrl.padEnd(225)
            , dim: 400
            , mtx: 1
            , ecl: "L"
            , ecb: 0
            , pal: ["#000000"]
            , vrb: 1

        });
        document.getElementById("qr-code").innerHTML = "";
        svgNode.removeAttribute("width");
        svgNode.removeAttribute("height");
        svgNode.setAttribute("width", "100%");
        
        document.getElementById("qr-code").append(svgNode);
        document.getElementById('qr-code-frame').style.display = "block";
        document.getElementById('qr-code').style.display = "block";
        document.getElementById('qr-code-frame').style.opacity = 1;
        document.getElementById('qr-code').style.opacity = 1;
        if (pin) {
            document.getElementById('pin-code-text').innerHTML = "Pin code: " + pin;
            document.getElementById('pin-code-text').style.display = "block";
        }     
    }
    function backToMainPage(){
        window.location.href = "/";
    }
    initView();
    
    runPresentation();
})();