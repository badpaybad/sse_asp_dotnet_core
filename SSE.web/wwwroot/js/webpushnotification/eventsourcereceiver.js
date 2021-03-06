PushServer = {
    __handlers: [],
    __currentEs: null,
    __mainChannel: [],
    __token: null,
    init: function (token) {
        PushServer.__token = token;
        PushServer.__handlers = [];
    },
    addHandler: function (handleNotification, mainChannel) {

        PushServer.__mainChannel[mainChannel] = mainChannel;

        PushServer.__handlers[mainChannel] = handleNotification;

        console.log("Subscribe: " + mainChannel);

        PushServer.listenChannel(mainChannel, function (msg) {
            // handleNotification(msg);
            PushServer.__handlers[mainChannel](msg);
        });

    },
    
    listenChannel: function (channel, onMessageReceived, onConnected, onOpen) {
        PushServer.__currentEs = new EventSource('/api/SSE/Listener?c=' + encodeURIComponent(channel)
            + '&token=' + encodeURIComponent(PushServer.__token));
        PushServer.__currentEs.onopen = function (evt) {
            if (onOpen) onOpen(evt);
        };
        PushServer.__currentEs.onconnected = function (evt) {
            if (onConnected) onConnected(evt);
            // console.log('connected');
        };
        PushServer.__currentEs.onmessage = function (evt) {

            onMessageReceived(evt.data);

            // var obj = evt.data;

            // for (var k in PushServer.__handlers) {
            //     PushServer.__handlers[k](obj);
            // }
            //console.log('onmessage');
        };
        PushServer.__currentEs.onerror = function (evt) {
            if (evt.currentTarget.readyState == 2 || PushServer.__currentEs.readyState == 2) {
                PushServer.listenChannel(channel, onMessageReceived, onConnected, onOpen);
                console.log('reconnected');
                console.log('onerror');
                console.log(evt);
            }
        };
    }
};
