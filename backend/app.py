from flask import Flask, request, jsonify, abort, Response
import json
import os
from validate import Validator
app = Flask(__name__)
DIR = os.path.dirname(os.path.realpath(__file__))


@app.route('/get')
def get_():
    content = read_content()

    l1 = request.args.get('l1')
    l2 = request.args.get('l2')
    l3 = request.args.get('l3')
    l4 = request.args.get('l4')
    l5 = request.args.get('l5')
    li = []

    for l in [l1, l2, l3, l4, l5]:
        if l != None:
            li.append(l)

    for l in li:
        if l not in content:
            print('GET: returning no key found')
            return 'no key found'
        content = content[l]

    print('GET: returning content')
    return jsonify(content)


@app.route('/set', methods=['POST'])
def set_():
    auth = request.json['authorization']
    if not authenticate(auth):
        abort(401)

    content = read_content()

    l1 = request.args.get('l1')
    l2 = request.args.get('l2')
    l3 = request.args.get('l3')
    l4 = request.args.get('l4')
    l5 = request.args.get('l5')
    li = []

    for l in [l1, l2, l3, l4, l5]:
        if l != None:
            li.append(l)
    # convert numeric values to integer
    li = [int(l) if l.isnumeric() else l for l in li]

    retval = content.copy()

    for l in li:
        if not str(l).isnumeric() and l not in content:
            print(f'SET: returning no key found (key={l})')
            return 'no key found'
        elif str(l).isnumeric() and int(l) >= len(content):
            content.append(content.copy()[int(l)-1])
            print(content)
            print('index out of range')
        content = content[l]
        print(f'-> {l}')

    new_content = request.json['data']
    print("\n\n\n", new_content)

    if len(li) == 0:
        retval = new_content
    elif len(li) == 1:
        retval[li[0]]['data'] = new_content
    elif len(li) == 2:
        retval[li[0]][li[1]]['data'] = new_content
    elif len(li) == 3:
        retval[li[0]][li[1]][li[2]]['data'] = new_content
    elif len(li) == 4:
        retval[li[0]][li[1]][li[2]][li[3]]['data'] = new_content
    elif len(li) == 5:
        retval[li[0]][li[1]][li[2]][li[3]][li[4]]['data'] = new_content

    remove_empty_menus(retval)

    write_content(retval)

    print('SET: returning success')
    return 'success'


@app.route('/addmenu', methods=['POST'])
def add_menu():
    for key in request.json:
        print(key, request.json[key])
    auth = request.json['authorization']
    if not authenticate(auth):
        abort(401)

    menu_title = request.args.get('title')

    data = { "title": {
        "data": menu_title,
        "type": "string"
        },
            "items":{
                "data": [
                    {
                        "ItemName": "",
                        "ItemImagePath": "",
                        "Description": ""
                        }
                    ],
                "type": "list"
                }
            }
    
    out_data = read_content()
    out_data['root']['menu']['menus'].append(data)
    write_content(out_data)

    return 'success'


@app.route('/validate', methods=['POST'])
def validate():
    v = Validator(os.path.join(DIR,'users.json'))
    name = request.json['name']
    pswd = request.json['password']
    if v.validate(name=name, pswd=pswd):
        return 'success'
    return 'error'

@app.route('/users', methods=['POST'])
def users():
    auth = request.json['authorization']
    if not authenticate(auth):
        abort(401)

    retval = []
    def GetUserInfo():
        with open(os.path.join(DIR,'users.json'), 'r') as f:
            return json.load(f)
    users = GetUserInfo()
    for user in users:
        retval.append(user['name'])
    return jsonify(retval)


@app.errorhandler(401)
def custom_401(error=''):
    return Response('unauthorized', 401, {'WWW-Authenticate': 'Basic realm="Login Required"'})

def authenticate(data: dict):
    with open(os.path.join(DIR,'authentication.json'), 'r') as f:
        json_data = json.load(f)
        return data['key'] == json_data['key']


def remove_empty_menus(data: dict) -> dict:
    new_menu_data = []
    for menu in data['root']['menu']['menus']:
        m = menu['items']['data']
        if len(m) != 0:
            new_menu_data.append(menu)
    data['root']['menu']['menus'] = new_menu_data
    return data

def read_content() -> dict:
    with open(os.path.join(DIR, 'content.json'), 'r') as f:
        return json.load(f)


def write_content(d: dict) -> dict:
    with open(os.path.join(DIR,'content.json'), 'w') as f:
        json.dump(d, f)
        

if __name__ == '__main__':
    app.run(debug=True)
