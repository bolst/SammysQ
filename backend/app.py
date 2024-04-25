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

    if verify_user(request.authorization) == False:
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

    retval = content.copy()

    for l in li:
        if l not in content:
            print('SET: returning no key found')
            return 'no key found'
        content = content[l]

    new_content = request.form.get('new_content')

    if len(li) == 0:
        retval = new_content
    elif len(li) == 1:
        retval[li[0]] = new_content
    elif len(li) == 2:
        retval[li[0]][li[1]] = new_content
    elif len(li) == 3:
        retval[li[0]][li[1]][li[2]] = new_content
    elif len(li) == 4:
        retval[li[0]][li[1]][li[2]][li[3]] = new_content
    elif len(li) == 5:
        retval[li[0]][li[1]][li[2]][li[3]][li[4]] = new_content

    write_content(retval)

    print('SET: returning success')
    return 'success'


@app.route('/validate', methods=['POST'])
def validate():
    v = Validator('users.json')
    name = request.json['name']
    pswd = request.json['password']
    if v.validate(name=name, pswd=pswd):
        return 'success'
    return 'error'


@app.errorhandler(401)
def custom_401(error):
    return Response('unauthorized', 401, {'WWW-Authenticate': 'Basic realm="Login Required"'})


def read_content() -> dict:
    with open(os.path.join(DIR, 'content.json'), 'r') as f:
        return json.load(f)


def write_content(d: dict) -> dict:
    with open(os.path.join(DIR,'content.json'), 'w') as f:
        json.dump(d, f)


if __name__ == '__main__':
    app.run(debug=True)
