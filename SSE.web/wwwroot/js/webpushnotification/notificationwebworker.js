self.importScripts('/js/webpushnotification/eventsourcereceiver.js');
var url = new URL(location);
const channel = url.searchParams.get('c');
const subscriber = url.searchParams.get('s');
const token = url.searchParams.get('token');

PushServer.init(token);

var connection = 0;

var _listMsg = [];

PushServer.addHandler(function (msg) {
  try {
    msg = JSON.parse(msg);

    _listMsg.push(msg);
  } catch (e) {
  }
}, channel, token);

function pushToUi() {
  var msg = _listMsg.pop();

  if (msg) {
      postMessage(msg);
  }

  setTimeout(() => {
    pushToUi();
  }, 500);
}

pushToUi();
